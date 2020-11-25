using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Extenject.Microsoft.Tests
{
    public class InjectionTests
    {
        private readonly IServiceCollection _services = new ExtenjectServiceCollection();

        [UnityTest]
        public IEnumerator EmptyTranslatorCanCreateServiceProvider()
        {
            yield return InitializeScene();

            var serviceProvider = Object.FindObjectOfType<TestInstaller>().ServiceProvider;

            Assert.IsInstanceOf<ExtenjectServiceProvider>(serviceProvider);
        }

        private IEnumerator InitializeScene()
        {
            SceneManager.LoadScene("Extenject.Microsoft.Tests/Scenes/SampleScene", LoadSceneMode.Single);

            TestInstaller.Services = _services;

            yield return null;
        }
    }
}
