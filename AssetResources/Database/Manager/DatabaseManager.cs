﻿// WARNING: Generated file. Do not modify!
using System;

namespace GameCore.Database
{
    public class DatabaseManager : Singleton<DatabaseManager>
    {
        private bool m_initializing;
        private bool m_initialized;
        private int m_initializingDatabaseCount;
        private Action m_onInitializeFinish;

        public void Initialize(Action onInitializeFinish = null)
        {
            m_onInitializeFinish = onInitializeFinish;

            if (m_initialized)
            {
                m_onInitializeFinish?.Invoke();
                return;
            }

            if (m_initializing)
            {
                return;
            }

            m_initializing = true;
            m_initializingDatabaseCount = 0;

            
        }

        private void OnDatabaseInitialized()
        {
            m_initializingDatabaseCount--;
            if (m_initializingDatabaseCount == 0)
            {
                m_initializing = false;
                m_initialized = true;
                m_onInitializeFinish?.Invoke();
            }
        }

        public T Load<T>(string key) where T : Data
        {
            return Database<T>.Load(key);
        }

        public T[] LoadAll<T>() where T : Data
        {
            return Database<T>.LoadAll();
        }

        public void LoadAsync<T>(string key, Action<T> onDataLoaded) where T : Data
        {
            Database<T>.LoadAsync(key, onDataLoaded);
        }

        public void LoadAllAsync<T>(Action<T[]> onDataLoaded) where T : Data
        {
            Database<T>.LoadAllAsync(onDataLoaded);
        }
    }
}