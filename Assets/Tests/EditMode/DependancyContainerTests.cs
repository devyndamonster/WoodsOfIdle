using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WoodsOfIdle;

public class DependancyContainerTests
{
    [Test]
    public void WillResolveImplementationTypeFromBoundType()
    {
        DependancyContainer container = new DependancyContainer();

        container.Bind<ITestClassA, TestClassA>();

        Assert.That(container.Resolve<ITestClassA>(), Is.TypeOf<TestClassA>());
    }

    [Test]
    public void WillResolveImplementationType()
    {
        DependancyContainer container = new DependancyContainer();

        container.Bind<TestClassA>();

        Assert.That(container.Resolve<TestClassA>(), Is.TypeOf<TestClassA>());
    }

    [Test]
    public void WillResolveGivenImplementation()
    {
        DependancyContainer container = new DependancyContainer();
        TestClassA testClassA = new TestClassA();

        container.Bind(testClassA);

        Assert.That(container.Resolve<TestClassA>(), Is.EqualTo(testClassA));
    }

    [Test]
    public void WillResolveGivenImplementationFromBoundType()
    {
        DependancyContainer container = new DependancyContainer();
        TestClassA testClassA = new TestClassA();

        container.Bind<ITestClassA>(testClassA);

        Assert.That(container.Resolve<ITestClassA>(), Is.EqualTo(testClassA));
    }

    [Test]
    public void WillInjectDependanciesIntoConstructor()
    {
        DependancyContainer container = new DependancyContainer();

        container.Bind<ITestClassA, TestClassA>();
        container.Bind<ITestClassB, TestClassB>();

        Assert.That(container.Resolve<ITestClassB>(), Is.TypeOf<TestClassB>());
        Assert.That(((TestClassB)container.Resolve<ITestClassB>()).classA, Is.TypeOf<TestClassA>());
    }

    [Test]
    public void WillCollectCorrectImplementations()
    {
        DependancyContainer container = new DependancyContainer();

        container.Bind<ITestClassA, TestClassA>();
        container.Bind<ITestClassB, TestClassB>();
        container.Bind<ITestClassC, TestClassC>();
        var implementations = container.CollectImplementationsOfType<ISharedTestInterface>();

        Assert.That(implementations.Count(), Is.EqualTo(2));
        Assert.That(implementations.OfType<TestClassA>().Count(), Is.EqualTo(1));
        Assert.That(implementations.OfType<TestClassB>().Count(), Is.EqualTo(1));
    }

    public class TestClassA : ITestClassA, ISharedTestInterface
    {
        
    }

    public class TestClassB : ITestClassB, ISharedTestInterface
    {
        public ITestClassA classA;

        public TestClassB(ITestClassA classA)
        {
            this.classA = classA;
        }
    }
    
    public class TestClassC : ITestClassC
    {

    }

    public interface ITestClassA { }

    public interface ITestClassB { }

    public interface ITestClassC { }

    public interface ISharedTestInterface { }

}
