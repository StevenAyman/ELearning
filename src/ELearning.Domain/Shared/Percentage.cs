using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Shared;
public sealed record Percentage
{
    private readonly double _percentage = (double) 1 / 100;
    public double Value { get; private set; }

    private Percentage(double percentage)
    {
        Value = percentage * _percentage;
    }

    public static Percentage Create(double value)
    {
        if (value < 0 || value > 100)
        {
            throw new ApplicationException("Invalid percentage value. Value should be between 0% and 100%");
        }

        return new Percentage(value);
    }
};
