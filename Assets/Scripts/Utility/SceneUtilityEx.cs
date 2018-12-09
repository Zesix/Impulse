using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Zenject;
using System;

namespace Impulse
{
	public static class SceneUtilityEx
	{
		public static string GetNextSceneName()
		{
			var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

			if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
			{
				return GetSceneNameByBuildIndex(nextSceneIndex);
			}

			return string.Empty;
		}

		private static string GetSceneNameByBuildIndex(int buildIndex)
		{
			return GetSceneNameFromScenePath(SceneUtility.GetScenePathByBuildIndex(buildIndex));
		}

		private static string GetSceneNameFromScenePath(string scenePath)
		{
			// Unity's asset paths always use '/' as a path separator
			var sceneNameStart = scenePath.LastIndexOf("/", StringComparison.Ordinal) + 1;
			var sceneNameEnd = scenePath.LastIndexOf(".", StringComparison.Ordinal);
			var sceneNameLength = sceneNameEnd - sceneNameStart;
			return scenePath.Substring(sceneNameStart, sceneNameLength);
		}
	}
}
