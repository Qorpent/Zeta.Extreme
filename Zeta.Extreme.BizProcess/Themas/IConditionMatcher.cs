﻿namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// Интерфейс резольвера кондиций
	/// </summary>
    public interface IConditionMatcher {
        /// <summary>
        /// True - при сходимости кондиций
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        bool IsConditionMatch(string condition);
    }
}