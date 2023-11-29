namespace Hmxs.Toolkit.Events
{
    /// <summary>
    /// 事件名索引
    /// </summary>
    public static class EventName
    {
        public static class General
        {
            public const string TestNoParamEvent = "TestNoParamEvent";
            public const string Test3ParamEvent = "Test3ParamEvent";
        }
        
        public static class Quest
        {
            public const string QuestStart = "QuestStart"; // 任务开始时
            public const string QuestAdvance = "QuestAdvance"; // 任务推进时
            public const string QuestFinish = "QuestFinish"; // 任务完成时
            public const string QuestStateChange = "QuestStateChange"; // 任务状态变化时
            public const string QuestStepDataUpdate = "QuestStepDataUpdate"; // StepData持久化

            public const string TestQuestOnCoinCollected = "TestQuestOnCoinCollected";
        }
        
        public static class Player
        {
            public const string SubmitPressed = "SubmitPressed";
        }
        
        public static class UI
        {
            public const string SceneEndFadeEffect = "SceneEndFadeEffect";
            public const string SceneStartFadeEffect = "SceneStartFadeEffect";
        }
    }
}