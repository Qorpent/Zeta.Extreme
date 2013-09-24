using System;

namespace Zeta.Extreme.Developer.MetaStorage.Tree
{
    /// <summary>
    /// Категория первичности формы
    /// </summary>
    [Flags]
    public enum PrimaryCompatibility
    {
        /// <summary>
        /// Форма в принципе не имеет первичных строк
        /// </summary>
        NoPrimary = 1<<0,
        /// <summary>
        /// Форма является полностью первичной, без формул и ссылок
        /// </summary>
        FullPrimary =1<<1,
        /// <summary>
        /// Все зависимости разрешаются локально
        /// </summary>
        SelfPrimary = 1<<2,
        /// <summary>
        /// Все зависимости разрешаются в рамках блока
        /// </summary>
        BlockPrimary = 1<<3,
        /// <summary>
        /// Первичная форма с зависимостями вне блока
        /// </summary>
        CrossBlock = 1<<4,
        /// <summary>
        /// Первичная форма с зависимостями вне подсистемы
        /// </summary>
        CrossSubsystem = 1<<5,
        /// <summary>
        /// Статус первичности не проверен и проигнорирован
        /// </summary>
        Ignored = 1<<6,
        /// <summary>
        /// Форма, которая может использоваться как первичная
        /// </summary>
        Primary = FullPrimary | SelfPrimary | BlockPrimary,
        /// <summary>
        /// Форма, которая может использоваться как первичная, но при этом воспринимается как проблема
        /// </summary>
        WarnPrimary = BlockPrimary,
        /// <summary>
        /// Форма,  которая имеет первичку, но которая не должна использоваться как превичная
        /// </summary>
        ErrorPrimary  = CrossBlock |CrossSubsystem,
    }
}
