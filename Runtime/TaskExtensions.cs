using System.Threading.Tasks;
using UnityEngine;

namespace JackSParrot.AddressablesEssentials
{
	public static class TaskExtensions
	{
		public static void HandleBackgroundException(this Task task)
		{
			task.ContinueWith(t =>
			{
				if (t.IsFaulted)
				{
					Debug.LogException(t.Exception);
				}
			}, TaskContinuationOptions.OnlyOnFaulted);
		}
	}
}
