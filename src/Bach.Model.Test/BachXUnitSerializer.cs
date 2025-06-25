// Module Name: BachXUnitSerializer.cs
// Project:     Bach.Model.Test
// Copyright (c) 2012, 2025  Eddie Velasquez.
//
// This source is subject to the MIT License.
// See http://opensource.org/licenses/MIT.
// All other rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to
// do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using Bach.Model;
using Bach.Model.Test;
using Xunit.Sdk;

[assembly:
  RegisterXunitSerializer(
    typeof( BachXUnitSerializer ),
    typeof( Accidental ),
    typeof( NoteName ),
    typeof( Pitch ),
    typeof( PitchClass ),
    typeof( Interval ),
    typeof( IntervalQuality ),
    typeof( ModeFormula )
  )]

namespace Bach.Model.Test;

using System.Diagnostics.CodeAnalysis;
using Xunit.Sdk;

internal class BachXUnitSerializer: IXunitSerializer
{
  #region Public Methods

  public object Deserialize(
    Type type,
    string serializedValue )
  {
    return type switch
    {
      not null when type == typeof( Accidental )      => (Accidental) int.Parse( serializedValue ),
      not null when type == typeof( NoteName )        => (NoteName) int.Parse( serializedValue ),
      not null when type == typeof( Pitch )           => (Pitch) int.Parse( serializedValue ),
      not null when type == typeof( PitchClass )      => (PitchClass) int.Parse( serializedValue ),
      not null when type == typeof( Interval )        => (Interval) int.Parse( serializedValue ),
      not null when type == typeof( IntervalQuality ) => (IntervalQuality) int.Parse( serializedValue ),
      not null when type == typeof( ModeFormula )     => (ModeFormula) int.Parse( serializedValue ),
      _                                               => throw new NotSupportedException( $"Type {type} is not supported." )
    };
  }

  public bool IsSerializable(
    Type type,
    object? value,
    [NotNullWhen( false )] out string? failureReason )
  {
    failureReason = null;

    return type == typeof( Accidental )
           || type == typeof( NoteName )
           || type == typeof( Pitch )
           || type == typeof( PitchClass )
           || type == typeof( Interval )
           || type == typeof( IntervalQuality )
           || type == typeof( ModeFormula );
  }

  public string Serialize(
    object value )
  {
    return value switch
    {
      Accidental accidental           => ( (int) accidental ).ToString(),
      NoteName noteName               => ( (int) noteName ).ToString(),
      Pitch pitch                     => ( (int) pitch ).ToString(),
      PitchClass pitchClass           => ( (int) pitchClass ).ToString(),
      Interval interval               => ( (int) interval ).ToString(),
      IntervalQuality intervalQuality => ( (int) intervalQuality ).ToString(),
      ModeFormula modeFormula         => ( (int) modeFormula ).ToString(),
      _                               => throw new NotSupportedException( $"Type {value.GetType()} is not supported." )
    };
  }

  #endregion
}
