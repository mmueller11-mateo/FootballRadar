using System.Text.Json.Serialization;

namespace FootballRadar.Business.Entities
{
    /// <summary>
    /// Data structure which contains the financial value of an object in a specific currency.
    /// </summary>
    /// <seealso cref="System.IEquatable{Seaprone.RealEstate.Domain.Price}" />

    public struct Money : IEquatable<Money>, ICloneable<Money>
    {
        [JsonIgnore]
        private const decimal CurrencyNeutralMoneyValue = decimal.Zero;

        /// <summary>
        /// Represents a currency which is global thus, allows money conversion from / to any currencies without any change in its value.
        /// <seealso cref="https://en.wikipedia.org/wiki/World_currency"/>
        /// <seealso cref="https://en.wikipedia.org/wiki/Terra_(currency)"/>
        /// </summary>
        [JsonIgnore]
        private const string GlobalCurrency = "Terra";

        /// <summary>
        /// Initializes a new instance of the <see cref="Money" /> class.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <exception cref="ArgumentException">Currency must be specified when the amount is not zero. - currency</exception>
        /// <exception cref="ArgumentOutOfRangeException">amount - The amount cannot be less than zero.</exception>
        public Money(decimal amount, string currency)
        {
            if (amount != CurrencyNeutralMoneyValue && string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException("Currency must be specified when the amount is not zero.", nameof(currency));
            }

            Amount = amount;
            Currency = currency.Trim();
        }

        /// <summary>
        /// Gets the amount of this monetary value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public decimal Amount { get; private set; }

        /// <summary>
        /// Gets the currency / unit (e.g. CHF, EUR) of this monetary value.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        public string Currency { get; private set; }

        /// <summary>
        /// Determines whether a currency has been defined.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if currency has been defined; otherwise, <c>false</c>.
        /// </returns>
        public bool IsCurrencyDefined()
        {
            return string.IsNullOrWhiteSpace(Currency) == false && string.Equals(Currency, GlobalCurrency, StringComparison.InvariantCultureIgnoreCase) == false;
        }

        /// <summary>
        /// Determines whether this monetary amount is currency neutral (e.g. has value of zero or a global / world currency).
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is currency neutral]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsCurrencyNeutral()
        {
            return Amount == CurrencyNeutralMoneyValue || string.IsNullOrWhiteSpace(Currency) || string.Equals(Currency, GlobalCurrency, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public Money Clone()
        {
            return new Money(Amount, Currency);
        }

        /// <summary>
        /// Creates 0 (zero) amount money with the specified currency.
        /// </summary>
        /// <remarks>
        /// If no currency is specified, then the global / world currency is used.
        /// </remarks>
        /// <param name="currency">The currency.</param>
        /// <returns></returns>
        public static Money Zero(string? currency = null)
        {
            return new Money(decimal.Zero, (currency ?? GlobalCurrency).Trim());
        }

        /// <summary>
        /// Creates currency neutral money.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Money CurrencyNeutral(decimal value)
        {
            return new Money(value, GlobalCurrency);
        }

        #region === Equability ===

        public override bool Equals(object other)
        {
            if (other is Money money)
            {
                return Equals(money);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Money other)
        {
            return VerifyCurrencyCompatibility(this, other) && Amount.Equals(other.Amount);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Amount, Currency).GetHashCode();
        }

        #endregion

        #region === Operator Overloading ===

        public static bool operator ==(Money left, Money right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Money left, Money right)
        {
            return !left.Equals(right);
        }

        public static Money operator +(Money left, Money right)
        {
            if (VerifyCurrencyCompatibility(left, right) == false)
            {
                throw new InvalidOperationException("Currency mismatch.");
            }

            return new Money(left.Amount + right.Amount, GetReferenceCurrency(left, right));
        }

        public static Money operator -(Money left, Money right)
        {
            if (VerifyCurrencyCompatibility(left, right) == false)
            {
                throw new InvalidOperationException("Currency mismatch.");
            }

            return new Money(left.Amount - right.Amount, GetReferenceCurrency(left, right));
        }

        public static Money operator *(Money left, int right)
        {
            return new Money(left.Amount * right, left.Currency);
        }

        public static Money operator *(Money left, decimal right)
        {
            return new Money(left.Amount * right, left.Currency);
        }

        public static bool operator <=(Money left, Money right)
        {
            if (VerifyCurrencyCompatibility(left, right) == false)
            {
                throw new InvalidOperationException("Currency mismatch.");
            }

            return left.Amount <= right.Amount;
        }

        public static bool operator >=(Money left, Money right)
        {
            if (VerifyCurrencyCompatibility(left, right) == false)
            {
                throw new InvalidOperationException("Currency mismatch.");
            }

            return left.Amount >= right.Amount;
        }

        public static bool operator <(Money left, Money right)
        {
            if (VerifyCurrencyCompatibility(left, right) == false)
            {
                throw new InvalidOperationException("Currency mismatch.");
            }

            return left.Amount < right.Amount;
        }

        public static bool operator >(Money left, Money right)
        {
            if (VerifyCurrencyCompatibility(left, right) == false)
            {
                throw new InvalidOperationException("Currency mismatch.");
            }

            return left.Amount > right.Amount;
        }

        #endregion

        public override string ToString()
        {
            return $"{Amount:N2} {Currency}";
        }

        private static bool VerifyCurrencyCompatibility(Money left, Money right)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            var currencyCompatible = !left.IsCurrencyDefined()
                                     || !right.IsCurrencyDefined()
                                     || left.IsCurrencyNeutral()
                                     || right.IsCurrencyNeutral()
                                     || string.Equals(left.Currency, right.Currency, StringComparison.InvariantCultureIgnoreCase);

            return currencyCompatible;
        }

        private static string GetReferenceCurrency(Money left, Money right)
        {
            return left.IsCurrencyDefined() ? left.Currency : right.Currency;
        }
    }
}
