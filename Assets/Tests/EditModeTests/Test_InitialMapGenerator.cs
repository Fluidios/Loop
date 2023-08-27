using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Test_InitialMapGenerator
{
    [Test]
    public void Test_RoadLoopGeneration()
    {
        //Assign
        var roadLoopGen = new InitialMapGenerator.MapGenerator();
        //Act
        try
        {
            var output = roadLoopGen.GenerateRoadLoop(new System.Random(), 25, 30, 4);
        }
        catch(System.Exception e)
        {
            Assert.Fail("InitialMapGenerator.MapGenerator.GenerateRoadLoop(new System.Random(), 50, 40, 5) is broken!\nException: " + e.Message);
        }
    }
}
