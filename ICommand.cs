namespace sample001
{
    /// <summary>
    /// コマンドパターンのインターフェース
    /// </summary>
    interface ICommand
    {
        void SetWork(WorkData target);

        void AddCount();

    }
}
