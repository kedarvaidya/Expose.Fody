## This is an add-in for [Fody](https://github.com/Fody/Fody/)

Exposes members and optionally implements interface of a field declared in class.

[Introduction to Fody](http://github.com/Fody/Fody/wiki/SampleUsage).


## Nuget 

Nuget package http://nuget.org/packages/Expose.Fody/

To Install from the Nuget Package Manager Console 
    
    PM> Install-Package Expose.Fody

## Your Code

    public interface ISample
    {
        int Foo { get; set; }

        int Bar(int x, int y);
    }

    public class SampleClass: ISample
    {
        public int Foo { get; set; }

        public int Bar(int x, int y)
        {
        	return x + y;
        }
    }

    public class SampleMembersExposer
    {
    	[ExposeMembers]
    	private ISample innerSample;

    	public SampleMembersExposer(SampleClass sample)
    	{
    		this.innerSample = sample;
    	}
    }

    public class SampleImplicitExposer
    {
    	[ExposeInterfaceImplicitly]
    	private ISample innerSample;

    	public SampleImplicitExposer(SampleClass sample)
    	{
    		this.innerSample = sample;
    	}
    }

    public class SampleExplicitExposer
    {
    	[ExposeInterfaceExplicitly]
    	private ISample innerSample;

    	public SampleExplicitExposer(SampleClass sample)
    	{
    		this.innerSample = sample;
    	}
    }

## What gets compiled

    public interface ISample
    {
        int Foo { get; set; }

        int Bar(int x, int y);
    }

    public class SampleClass: ISample
    {
        public int Foo { get; set; }

        public int Bar(int x, int y)
        {
        	return x + y;
        }
    }

    public class SampleMembersExposer
    {
    	private ISample innerSample;

    	public SampleMembersExposer(SampleClass sample)
    	{
    		this.innerSample = sample;
    	}

    	public int Foo 
    	{ 
    		get { return innerSample.Foo; } 
    		set { innerSample.Foo = value; }
    	}

        public int Bar(int x, int y)
        {
        	return innerSample.Bar(x, y);
        }
    }

    public class SampleImplicitExposer: ISample
    {
    	private ISample innerSample;

    	public SampleImplicitExposer(SampleClass sample)
    	{
    		this.innerSample = sample;
    	}

    	public int Foo 
    	{ 
    		get { return innerSample.Foo; } 
    		set { innerSample.Foo = value; }
    	}

        public int Bar(int x, int y)
        {
        	return innerSample.Bar(x, y);
        }
    }

    public class SampleExplicitExposer: ISample
    {
    	private ISample innerSample;

    	public SampleExplicitExposer(SampleClass sample)
    	{
    		this.innerSample = sample;
    	}

    	int ISample.Foo 
    	{ 
    		get { return innerSample.Foo; } 
    		set { innerSample.Foo = value; }
    	}

        int ISample.Bar(int x, int y)
        {
        	return innerSample.Bar(x, y);
        }
    }