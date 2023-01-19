using UnityEngine;

namespace Runner
{
    public class MyClassBase
    {
        public int IntBase;

        public void Init()
        {
            MyClassBase classBase = new MyClassBase();
            classBase.IntBase = 10;
            DoSomething(classBase);

            MyClassBase classA = new MyClassA();
            classA.IntBase = 10;
            // classA.IntA = 20;
            DoSomething(classA);

            MyClassBase classFromA = new MyClassFromA();
            classFromA.IntBase = 10;
            // classFromA.IntA = 20;
            // classFromA.IntA2 = 30;
            DoSomething(classFromA);
        }

        public void DoSomething(MyClassBase classBase)
        {
            MyClassA classA = classBase as MyClassA;
            MyClassFromA classFromA = classBase as MyClassFromA;
            Debug.Log(classA);
            Debug.Log(classFromA);

            if (classBase is MyClassA a)
            {
                Debug.Log(a);
            }

            if (classBase is MyClassFromA fromA)
            {
                Debug.Log(fromA);
            }
        }
    }

    public class MyClassA : MyClassBase
    {
        public int IntA;
    }

    public struct MyStruct
    {
        public int Int;
    }

    public class MyClassFromA : MyClassA
    {
        public int IntA2;
    }

    public class MyClassB : MyClassBase
    {
        public int IntB;
    }

    public class MyClassFromB : MyClassB
    {
        public int IntB2;
    }
}