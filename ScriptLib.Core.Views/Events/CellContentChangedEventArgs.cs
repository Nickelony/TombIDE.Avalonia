﻿using System;

namespace ScriptLib.Core.Views.Events
{
	public class CellContentChangedEventArgs : EventArgs
	{
		public int ColumnIndex { get; }
		public int RowIndex { get; }
		public object OldValue { get; }
		public object NewValue { get; set; }

		public CellContentChangedEventArgs(int columnIndex, int rowIndex, object oldValue, object newValue)
		{
			ColumnIndex = columnIndex;
			RowIndex = rowIndex;
			OldValue = oldValue;
			NewValue = newValue;
		}
	}
}
