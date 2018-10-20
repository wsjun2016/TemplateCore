using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BasisFrameWork.Extension;
using BasisFrameWork.Utilities.Configuration;

namespace BasisFrameWork.Utilities.Tasks
{
    /// <summary>
    /// 任务总数控制
    /// </summary>
    public class TaskGroup {
        public TaskGroup() { }

        /// <summary>
        /// 允许执行任务的最大数量
        /// </summary>
        public int TaskMaxCount {
            get {
                int totalCount = Cfg.Get("TotalCountOfTaskGroup").ToInt32();
                return totalCount <= 0 ? 5 : totalCount;
            }
        }

        private volatile Dictionary<int, Task> _currentTasks = new Dictionary<int, Task>();

        /// <summary>
        /// 当前执行任务的个数
        /// </summary>
        public int CurrentTasksCount => _currentTasks.Count;

        /// <summary>
        /// 新增任务,并返回是否新增成功的标志.
        /// </summary>
        /// <param name="task"></param>
        /// <returns>返回是否新增成功的标志 true:成功  false:失败</returns>
        public bool AddTask(Task task) {
            bool rc = false;
            if (CurrentTasksCount < TaskMaxCount && task != null) {
                _currentTasks.Add(task.Id, task);
                rc = true;
            }
            return rc;
        }

        /// <summary>
        /// 运行任务
        /// </summary>
        public void RunTasks() {
            if (_currentTasks != null && _currentTasks.Count > 0) {
                List<Task> tasks = new List<Task>();
                foreach (var key in _currentTasks.Keys) {
                    //只执行 新创建的任务
                    if (_currentTasks[key].Status == TaskStatus.Created) {
                        _currentTasks[key].ContinueWith(t => {
                            _currentTasks.Remove(key);
                            t.Dispose();
                        });
                        tasks.Add(_currentTasks[key]);
                    }
                }
                tasks.ForEach(it => { it.Start(); });
            }
        }
    }
}
