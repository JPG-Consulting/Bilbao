#if NET35
//------------------------------------------------------------------------------
// <copyright file="ClientIDModes.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
 
using System.Diagnostics.CodeAnalysis;

namespace System.Web.UI
{
    [SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase")]
    public enum ClientIDMode
    {
        Inherit=0,

        [SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase")]
        AutoID = 1,

        Predictable = 2,

        Static = 3
    }
}
#endif