using System.Diagnostics;

namespace ObjectPool {

    public class BulletEntity {
        public int id;
        public int typeID;
        public int hp;
        public int hpMax;
        public float speed;
        public float dmg;
        // 球 3600 顶点 * 3
        public float[] verices = new float[3600 * 3];
    }

    public static class Program {

        public static void Main() {

            const int COUNT = 1000;
            Pool<BulletEntity> pool = new Pool<BulletEntity>(Factory_CreateFunc, COUNT);

            Stopwatch sw = new Stopwatch();

            // 为什么要用对象池?
            // 我们在游戏里一帧内 16.67ms
            // 而生成对象/销毁会消耗很多时间
            // 因此我们要减少 生成与销毁 的次数

            // 在游戏打开时, 就生成足额的对象(内存), 后续就不用开辟与释放
            // 保证在循环时占用更少时间

            List<BulletEntity> repository = new List<BulletEntity>(COUNT);

            // ==== 1. 不用对象池 ====
            sw.Start();

            // - 生成
            for (int i = 0; i < COUNT; i += 1) {
                BulletEntity bullet = new BulletEntity();
                repository.Add(bullet);
            }

            // - 销毁 (产生GC)
            repository.Clear();

            sw.Stop();
            Console.WriteLine("不用对象池: " + sw.Elapsed.TotalMilliseconds + "ms");

            // ==== 2. 使用对象池 ====
            sw.Reset();
            sw.Start();

            // - 生成
            for (int i = 0; i < COUNT; i += 1) {
                BulletEntity bullet = pool.Take(); // 取出
                repository.Add(bullet);
            }

            // - 销毁
            for (int i = 0; i < COUNT; i += 1) {
                pool.Return(repository[i]); // 放回去
            }
            repository.Clear();

            sw.Stop();
            Console.WriteLine("使用对象池: " + sw.Elapsed.TotalMilliseconds + "ms");

        }

        static BulletEntity Factory_CreateFunc() {
            return new BulletEntity();
        }

    }

}