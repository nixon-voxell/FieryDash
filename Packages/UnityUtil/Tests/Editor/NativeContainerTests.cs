using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using NUnit.Framework;
using Voxell.Mathx;

namespace Voxell.NativeContainers
{
  public class NativeContainerTests
  {
    private const int BATCH_SIZE = 16;
    private const int LOOP_SIZE = BATCH_SIZE*8;
    [Test]
    public void AtomicIncrement()
    {
      NativeIncrement nativeIncrement = new NativeIncrement(Allocator.TempJob);
      AtomicIncrementJob atomicAddJob = new AtomicIncrementJob { nativeIncrement = nativeIncrement };

      JobHandle jobHandle = atomicAddJob.Schedule(LOOP_SIZE, BATCH_SIZE);
      jobHandle.Complete();

      Assert.AreEqual(nativeIncrement.Count, LOOP_SIZE);

      nativeIncrement.Dispose();
    }

    [BurstCompile(CompileSynchronously = true)]
    private struct AtomicIncrementJob : IJobParallelFor
    {
      public NativeIncrement nativeIncrement;

      public void Execute(int index)
      {
        nativeIncrement.Increment();
      }
    }

    [Test]
    public void AtomicAdd()
    {
      JobHandle jobHandle;
      NativeAdd nativeAdd = new NativeAdd(Allocator.TempJob);
      AtomicAddJob atomicAddJob = new AtomicAddJob { nativeAdd = nativeAdd };

      for (int i=0; i < 5; i++)
      {
        atomicAddJob.addAmount = i;

        jobHandle = atomicAddJob.Schedule(LOOP_SIZE, BATCH_SIZE);
        jobHandle.Complete();

        Assert.AreEqual(nativeAdd.Value, LOOP_SIZE*i);
        nativeAdd.Value = 0;
      }

      nativeAdd.Dispose();
    }

    [BurstCompile(CompileSynchronously = true)]
    private struct AtomicAddJob : IJobParallelFor
    {
      public int addAmount;
      public NativeAdd nativeAdd;

      public void Execute(int index)
      {
        nativeAdd.AtomicAdd(addAmount);
      }
    }

    [Test]
    public void AtomicExchange()
    {
      int[] seqArray = MathUtil.GenerateSeqArray(LOOP_SIZE);
      bool[] seqCheck = new bool[LOOP_SIZE];
      for (int i=0; i < LOOP_SIZE; i++) seqCheck[i] = false;

      NativeExchange nativeExchange = new NativeExchange(Allocator.TempJob);
      NativeArray<int> na_indices = new NativeArray<int>(seqArray, Allocator.TempJob);
      NativeArray<int> na_exchangedIndices = new NativeArray<int>(LOOP_SIZE, Allocator.TempJob);

      AtomicExchangeJob atomicExchangeJob = new AtomicExchangeJob
      { nativeExchange = nativeExchange, na_indices = na_indices, na_exchangedIndices = na_exchangedIndices };

      JobHandle jobHandle = atomicExchangeJob.Schedule(LOOP_SIZE, BATCH_SIZE);
      jobHandle.Complete();

      for (int i=0; i < LOOP_SIZE; i++)
      {
        // assign true to whichever exchange index it get
        seqCheck[na_exchangedIndices[i]] = true;
      }
      // lastly, we account for the last exchanged value
      seqCheck[nativeExchange.Value] = true;

      nativeExchange.Dispose();
      na_indices.Dispose();
      na_exchangedIndices.Dispose();

      // check if all indices has been successfully exchanged
      for (int i=0; i < LOOP_SIZE; i++)
        Assert.True(seqCheck[i]);
    }

    [BurstCompile(CompileSynchronously = true)]
    private struct AtomicExchangeJob : IJobParallelFor
    {
      public NativeExchange nativeExchange;
      public NativeArray<int> na_indices;
      public NativeArray<int> na_exchangedIndices;

      public void Execute(int index)
      {
        int initialValue = nativeExchange.AtomicExchange(index);
        na_exchangedIndices[index] = initialValue;
      }
    }
  }
}