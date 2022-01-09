/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2022 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1470.1000.M14.211205-1900

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TreeOfLife.Core;
using TreeOfLife.Core.Taxonomy;
using TreeOfLife.Core.Taxonomy.Extensions;

namespace TreeOfLife.UI
{
    // 为UI提供Core方法的异步调用。
    public static class AsyncMethod
    {
        public static Action Start { get; set; } // 异步调用开始时执行的操作。
        public static Action Finish { get; set; } // 异步调用完成时执行的操作。

        //

        #region 文件操作

        public static async Task<bool> OpenAsync(string fileName)
        {
            Start();
            bool result = await Task.Run(() => Entrance.Open(fileName));
            Finish();

            return result;
        }

        public static async Task<bool> SaveAsync()
        {
            Start();
            bool result = await Task.Run(Entrance.Save);
            Finish();

            return result;
        }

        public static async Task<bool> SaveAsAsync(string fileName)
        {
            Start();
            bool result = await Task.Run(() => Entrance.SaveAs(fileName));
            Finish();

            return result;
        }

        public static async Task<bool> CloseAsync()
        {
            Start();
            bool result = await Task.Run(Entrance.Close);
            Finish();

            return result;
        }

        #endregion

        #region 类群编辑

        public static async Task SetParentAsync(this Taxon taxon, Taxon parent)
        {
            Start();
            await Task.Run(() => taxon.SetParent(parent));
            Finish();
        }

        public static async Task<Taxon> AddParentUplevelAsync(this Taxon taxon)
        {
            Start();
            Taxon result = await Task.Run(() => taxon.AddParentUplevel());
            Finish();

            return result;
        }

        public static async Task<Taxon> AddParentDownlevelAsync(this Taxon taxon)
        {
            Start();
            Taxon result = await Task.Run(() => taxon.AddParentDownlevel());
            Finish();

            return result;
        }

        public static async Task MoveChildAsync(this Taxon taxon, int oldIndex, int newIndex)
        {
            Start();
            await Task.Run(() => taxon.MoveChild(oldIndex, newIndex));
            Finish();
        }

        public static async Task SwapChildAsync(this Taxon taxon, int index1, int index2)
        {
            Start();
            await Task.Run(() => taxon.SwapChild(index1, index2));
            Finish();
        }

        public static async Task<Taxon> AddChildAsync(this Taxon taxon)
        {
            Start();
            Taxon result = await Task.Run(() => taxon.AddChild());
            Finish();

            return result;
        }

        public static async Task AddExcludeAsync(this Taxon taxon, Taxon exclude)
        {
            Start();
            await Task.Run(() => taxon.AddExclude(exclude));
            Finish();
        }

        public static async Task RemoveExcludeAsync(this Taxon taxon, Taxon exclude)
        {
            Start();
            await Task.Run(() => taxon.RemoveExclude(exclude));
            Finish();
        }

        public static async Task<int> GetIndexOfExcludeAsync(this Taxon taxon, Taxon exclude)
        {
            Start();
            int result = await Task.Run(() => taxon.GetIndexOfExclude(exclude));
            Finish();

            return result;
        }

        public static async Task MoveExcludeAsync(this Taxon taxon, int oldIndex, int newIndex)
        {
            Start();
            await Task.Run(() => taxon.MoveExclude(oldIndex, newIndex));
            Finish();
        }

        public static async Task SwapExcludeAsync(this Taxon taxon, int index1, int index2)
        {
            Start();
            await Task.Run(() => taxon.SwapExclude(index1, index2));
            Finish();
        }

        public static async Task AddIncludeAsync(this Taxon taxon, Taxon include)
        {
            Start();
            await Task.Run(() => taxon.AddInclude(include));
            Finish();
        }

        public static async Task RemoveIncludeAsync(this Taxon taxon, Taxon include)
        {
            Start();
            await Task.Run(() => taxon.RemoveInclude(include));
            Finish();
        }

        public static async Task<int> GetIndexOfIncludeAsync(this Taxon taxon, Taxon include)
        {
            Start();
            int result = await Task.Run(() => taxon.GetIndexOfInclude(include));
            Finish();

            return result;
        }

        public static async Task MoveIncludeAsync(this Taxon taxon, int oldIndex, int newIndex)
        {
            Start();
            await Task.Run(() => taxon.MoveInclude(oldIndex, newIndex));
            Finish();
        }

        public static async Task SwapIncludeAsync(this Taxon taxon, int index1, int index2)
        {
            Start();
            await Task.Run(() => taxon.SwapInclude(index1, index2));
            Finish();
        }

        public static async Task RemoveCurrentAsync(this Taxon taxon, bool removeChildren)
        {
            Start();
            await Task.Run(() => taxon.RemoveCurrent(removeChildren));
            Finish();
        }

        public static async Task ParseCurrentAsync(this Taxon taxon, string name)
        {
            Start();
            await Task.Run(() => taxon.ParseCurrent(name));
            Finish();
        }

        public static async Task ParseChildrenAsync(this Taxon taxon, params string[] names)
        {
            Start();
            await Task.Run(() => taxon.ParseChildren(names));
            Finish();
        }

        #endregion
    }
}