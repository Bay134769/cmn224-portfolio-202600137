using NUnit.Framework;
using FeeSystem;
using System;
using System.Collections.Generic;

namespace FeeSystem.Tests;

[TestFixture]
public class FeeCalculatorTests
{
    [Test]
    public void OutstandingBalance_NoPayments_ReturnsFullFee()
    {
        var calc = new FeeCalculator();
        var result = calc.OutstandingBalance(600m, new List<decimal>());
        Assert.That(result, Is.EqualTo(600m));
    }

    [Test]
    public void OutstandingBalance_OnePayment_ReturnsRemainingBalance()
    {
        var calc = new FeeCalculator();
        var result = calc.OutstandingBalance(600m, new List<decimal> { 200m });
        Assert.That(result, Is.EqualTo(400m));
    }

    [Test]
    public void OutstandingBalance_MultiplePayments_ReturnsCorrectBalance()
    {
        var calc = new FeeCalculator();
        var result = calc.OutstandingBalance(
            600m,
            new List<decimal> { 200m, 200m, 100m });

        Assert.That(result, Is.EqualTo(100m));
    }

    [Test]
    public void OutstandingBalance_FullyPaid_ReturnsZero()
    {
        var calc = new FeeCalculator();
        var result = calc.OutstandingBalance(
            600m,
            new List<decimal> { 600m });

        Assert.That(result, Is.EqualTo(0m));
    }

    [Test]
    public void OutstandingBalance_Overpayment_ReturnsNegativeBalance()
    {
        var calc = new FeeCalculator();
        var result = calc.OutstandingBalance(
            600m,
            new List<decimal> { 700m });

        Assert.That(result, Is.EqualTo(-100m));
    }

    [Test]
    public void OutstandingBalance_NegativeFee_ThrowsException()
    {
        var calc = new FeeCalculator();

        Assert.That(
            () => calc.OutstandingBalance(-1m, new List<decimal>()),
            Throws.ArgumentException);
    }

    [Test]
    public void IsClearedForExams_HalfPaid_ReturnsTrue()
    {
        var calc = new FeeCalculator();

        var result = calc.IsClearedForExams(
            600m,
            new List<decimal> { 300m });

        Assert.That(result, Is.True);
    }

    [Test]
    public void IsClearedForExams_OneToeaUnderHalf_ReturnsFalse()
    {
        var calc = new FeeCalculator();

        var result = calc.IsClearedForExams(
            600m,
            new List<decimal> { 299.99m });

        Assert.That(result, Is.False);
    }
}