// Copyright 2023 Niantic, Inc. All Rights Reserved.

using System;

namespace CustomizeInput
{
    /// <summary>
    /// Input event lifecycle descriptor
    /// </summary>
    internal enum InputPhase
    {
        Began,
        Held,
        Ended,
        Hovered,
        Canceled
    }
}
