﻿namespace EzBus
{
    public interface ITaskRunner
    {
        void RunStartupTasks();
        void RunShutdownTasks();
    }
}