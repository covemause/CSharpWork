namespace sample001
{
    /// <summary>
    /// 偶数を処理するコマンド
    /// </summary>
    class cmdPatternA : cmdBase
    {
        protected WorkData wd;

        public override void SetWork(WorkData target)
        {
            this.wd = target;
        }

        public override void AddCount()
        {
            this.wd.even += 1;
        }
    }
}
