const Peer = window.Peer;

(async function main() {
	const joinTrigger = document.getElementById("js-join-trigger");
	const leaveTrigger = document.getElementById("js-leave-trigger");
	const remoteVideos = document.getElementById("js-remote-streams");

	const messages = document.getElementById("js-messages");

	// eslint-disable-next-line require-atomic-updates
	const peer = (window.peer = new Peer({
		key: "96b84e6e-573d-4cd4-8c7b-2200d826eef5",
		debug: 3,
	}));

	// Register join handler
	joinTrigger.addEventListener("click", () => {
		// Note that you need to ensure the peer has connected to signaling server
		// before using methods of peer instance.
		if (!peer.open) {
			return;
		}

		const room = peer.joinRoom("csl1", {
			mode: "mesh",
		});

		// Render remote stream for new peer join in the room
		room.on("stream", async (stream) => {
			const newVideo = document.createElement("video");
			newVideo.srcObject = stream;
			newVideo.playsInline = true;
			// mark peerId to find it later at peerLeave event
			newVideo.setAttribute("data-peer-id", stream.peerId);
			remoteVideos.append(newVideo);
			await newVideo.play().catch(console.error);
		});

		// for closing room members
		room.on("peerLeave", (peerId) => {
			const remoteVideo = remoteVideos.querySelector(
				`[data-peer-id="${peerId}"]`
			);
			remoteVideo.srcObject.getTracks().forEach((track) => track.stop());
			remoteVideo.srcObject = null;
			remoteVideo.remove();
		});

		// for closing myself
		room.once("close", () => {
			Array.from(remoteVideos.children).forEach((remoteVideo) => {
				remoteVideo.srcObject.getTracks().forEach((track) => track.stop());
				remoteVideo.srcObject = null;
				remoteVideo.remove();
			});
		});

		leaveTrigger.addEventListener("click", () => room.close(), { once: true });
	});

	peer.on("error", console.error);
})();
