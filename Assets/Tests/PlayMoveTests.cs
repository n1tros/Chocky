using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PlayMoveTests {

	[Test]
	public void PlayMoveTestsSimplePasses() {
        // Use the Assert class to test conditions.#
        Assert.AreEqual(0, 0);
	}

    

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator PlayMoveTestsWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
