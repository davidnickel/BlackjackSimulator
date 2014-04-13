using System;

namespace Domain
{
    public class BasicStrategyFactory : IBasicStrategyFactory
    {
        #region IBasicStrategyFactory Members

        public static IBasicStrategy GetInstance(TableRules rules)
        {
            if (rules.NumberOfDecks == 4)
            {
                return FourDeckBasicStrategy.Instance;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
