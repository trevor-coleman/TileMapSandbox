using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public interface IBuildCommand
{
    void Execute();
}
    
public class BuildCommand<T> : IBuildCommand where T: IBuildableData
{
    private Builder builder;
    private readonly IBuildableData buildableData;

    public BuildCommand(Builder builder, T buildableData)
    {
        this.builder = builder;
        this.buildableData = buildableData;

    }

    public void Execute()
    {
        builder.Build(buildableData);
    }
}

public class DemolishCommand<T> : IBuildCommand where T: IBuildableData
{
    private Builder builder;
    private readonly IBuildableData buildableData;

    public DemolishCommand(Builder builder, T buildableData)
    {
        this.builder = builder;
        this.buildableData = buildableData;

    }

    public void Execute()
    {
        builder.Demolish(buildableData);
    }
}

