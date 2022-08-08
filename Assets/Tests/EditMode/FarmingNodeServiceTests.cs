using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WoodsOfIdle;

public class FarmingNodeServiceTests
{
    private IFarmingNodeService farmingNodeService = new FarmingNodeService();

    [Test]
    public void WillFarmTenItemsInTenSeconds()
    {
        DateTime timeLastHarvested = DateTime.Now;
        DateTime timeOfHarvest = timeLastHarvested.AddSeconds(10.1);

        FarmingNodeState nodeState = new FarmingNodeState
        {
            IsActive = true,
            TimeLastHarvested = timeLastHarvested,
            TimeToHarvest = 1
        };

        int numberOfHarvests = farmingNodeService.CalculateNumberOfHarvests(nodeState, timeOfHarvest);

        Assert.That(numberOfHarvests, Is.EqualTo(10));
    }

    [Test]
    public void WillSetTimeLastHarvestedCorrectly()
    {
        DateTime timeLastHarvested = DateTime.Now;
        DateTime tenSecondsLater = timeLastHarvested.AddSeconds(10);

        FarmingNodeState nodeState = new FarmingNodeState
        {
            IsActive = true,
            TimeLastHarvested = timeLastHarvested,
            TimeToHarvest = 1
        };

        DateTime calculatedTimeLastHarvested = farmingNodeService.CalculateLastHarvestTime(nodeState, 10);

        Assert.That(calculatedTimeLastHarvested, Is.EqualTo(tenSecondsLater));
    }

    [Test]
    public void WontFarmWhenInactive()
    {
        DateTime timeLastHarvested = DateTime.Now;
        DateTime timeOfHarvest = timeLastHarvested.AddSeconds(10.1);

        FarmingNodeState nodeState = new FarmingNodeState
        {
            IsActive = false,
            TimeLastHarvested = timeLastHarvested,
            TimeToHarvest = 1
        };

        int numberOfHarvests = farmingNodeService.CalculateNumberOfHarvests(nodeState, timeOfHarvest);

        Assert.That(numberOfHarvests, Is.EqualTo(0));
    }

    [Test]
    public void WontFarmFromBeforeReactivated()
    {
        DateTime timeOfReactivation = DateTime.Now;
        DateTime timeLastHarvested = timeOfReactivation.AddSeconds(-10);

        FarmingNodeState nodeState = new FarmingNodeState
        {
            IsActive = false,
            TimeLastHarvested = timeLastHarvested,
            TimeToHarvest = 1
        };

        farmingNodeService.SetNodeActiveState(nodeState, true);
        int numberOfHarvests = farmingNodeService.CalculateNumberOfHarvests(nodeState, timeOfReactivation);

        Assert.That(numberOfHarvests, Is.EqualTo(0));
    }


}
