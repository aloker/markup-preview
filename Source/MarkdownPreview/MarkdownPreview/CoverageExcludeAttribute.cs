#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

using System;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
public sealed class CoverageExcludeAttribute : Attribute
{
}