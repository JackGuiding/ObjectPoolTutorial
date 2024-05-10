using System;
using System.Collections.Generic;

namespace ObjectPool {

    public class Pool<T> where T : class {

        // 不用 List, 是因为 Stack 更方便且够用
        Stack<T> stack;
        Func<T> createFunc;

        public Pool(Func<T> createFunc, int count) {

            this.createFunc = createFunc;

            // 预先生成好了一堆对象
            stack = new Stack<T>(count);
            for (int i = 0; i < count; i++) {
                stack.Push(createFunc.Invoke());
            }

        }

        public T Take() {
            if (stack.Count > 0) {
                return stack.Pop();
            } else {
                return createFunc.Invoke();
            }
        }

        public void Return(T obj) {
            stack.Push(obj);
        }

    }

}