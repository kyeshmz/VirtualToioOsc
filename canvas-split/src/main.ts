//@ts-ignore
// @ts-nocheck

// let media;
// let canvas;

const videoElement = document.querySelector("video");

const videoSelect = document.querySelector("select#videoSource");
const selectors = [videoSelect];
const canvas1 = document.getElementById("canvasPreview1") as HTMLCanvasElement;
const canvas2 = document.getElementById("canvasPreview2") as HTMLCanvasElement;
const canvasCtx = canvas1.getContext("2d");
const canvasCtx2 = canvas2.getContext("2d");

function gotDevices(deviceInfos) {
	// Handles being called several times to update labels. Preserve values.
	const values = selectors.map((select) => select.value);
	selectors.forEach((select) => {
		while (select.firstChild) {
			select.removeChild(select.firstChild);
		}
	});
	for (let i = 0; i !== deviceInfos.length; ++i) {
		const deviceInfo = deviceInfos[i];
		const option = document.createElement("option");
		option.value = deviceInfo.deviceId;

		if (deviceInfo.kind === "videoinput") {
			option.text = deviceInfo.label || `camera ${videoSelect.length + 1}`;
			videoSelect.appendChild(option);
		} else {
			console.log("Some other kind of source/device: ", deviceInfo);
		}
	}
	selectors.forEach((select, selectorIndex) => {
		if (
			Array.prototype.slice
				.call(select.childNodes)
				.some((n) => n.value === values[selectorIndex])
		) {
			select.value = values[selectorIndex];
		}
	});
}
function handleError(error) {
	console.log(
		"navigator.MediaDevices.getUserMedia error: ",
		error.message,
		error.name
	);
}

navigator.mediaDevices.enumerateDevices().then(gotDevices).catch(handleError);

// Attach audio output device to video element using device/sink ID.
function attachSinkId(element, sinkId) {
	if (typeof element.sinkId !== "undefined") {
		element
			.setSinkId(sinkId)
			.then(() => {
				console.log(`Success, audio output device attached: ${sinkId}`);
			})
			.catch((error) => {
				let errorMessage = error;
				if (error.name === "SecurityError") {
					errorMessage = `You need to use HTTPS for selecting audio output device: ${error}`;
				}
				console.error(errorMessage);
				// Jump back to first output device in the list as it's the default.
				audioOutputSelect.selectedIndex = 0;
			});
	} else {
		console.warn("Browser does not support output device selection.");
	}
}

function changeAudioDestination() {
	const audioDestination = audioOutputSelect.value;
	attachSinkId(videoElement, audioDestination);
}

function gotStream(stream) {
	window.stream = stream; // make stream available to console
	videoElement.srcObject = stream;

	// Refresh button list in case labels have become available
	return navigator.mediaDevices.enumerateDevices();
}

function start() {
	if (window.stream) {
		window.stream.getTracks().forEach((track) => {
			track.stop();
		});
	}
	// const audioSource = audioInputSelect.value;
	const videoSource = videoSelect.value;
	const constraints = {
		// audio: { deviceId: audioSource ? { exact: audioSource } : undefined },
		video: { deviceId: videoSource ? { exact: videoSource } : undefined },
	};
	navigator.mediaDevices
		.getUserMedia(constraints)
		.then(gotStream)
		.then(gotDevices)

		// .then(_canvasUpdate)
		.catch(handleError);
}

// audioInputSelect.onchange = start;
// audioOutputSelect.onchange = changeAudioDestination;

videoSelect.onchange = start;

start();

// canvas要素をつくる

// canvas1.width = videoElement?.width;
// canvas1.height = videoElement?.height;

// コンテキストを取得する

// video要素の映像をcanvasに描画する
canvasUpdate();

function canvasUpdate() {
	canvas1.width = videoElement?.width / 2;
	canvas1.height = videoElement?.height;
	canvasCtx.drawImage(videoElement, 0, 0, 1920, 1080);
	requestAnimationFrame(canvasUpdate);
}
canvasUpdate2();

function canvasUpdate2() {
	canvas2.width = videoElement?.width / 2;
	canvas2.height = videoElement?.height;
	canvasCtx2.drawImage(videoElement, -960, 0, 1920, 1080);
	requestAnimationFrame(canvasUpdate2);
}

let room1, room2;
function sendRTC() {
	const joinTrigger = document.getElementById("send-btn");

	// eslint-disable-next-line require-atomic-updates
	peer = window.peer = new Peer({
		key: "96b84e6e-573d-4cd4-8c7b-2200d826eef5",
		debug: 3,
	});

	// // Register join handler
	joinTrigger.addEventListener("click", () => {
		//   // Note that you need to ensure the peer has connected to signaling server
		//   // before using methods of peer instance.
		if (!peer.open) {
			console.log("no peer");
			return;
		}

		const canvas1 = document.getElementById("canvasPreview1");
		const stream1 = canvas1.captureStream(30);
		if (room1 || room2) return;
		// const title = document.getElementById("title");
		// title.innerHTML = "STARTED SKYWAY";

		room1 = peer.joinRoom("csl1", {
			mode: "mesh",
			stream: stream1,
			audioReceiveEnabled: false,
			videoReceiveEnabled: false,
		});
		console.log(room1);
		const canvas2 = document.getElementById("canvasPreview2");
		const stream2 = canvas2.captureStream(30);

		room2 = peer.joinRoom("csl2", {
			mode: "mesh",
			stream: stream2,
			audioReceiveEnabled: false,
			videoReceiveEnabled: false,
		});
		console.log(room2);
	});
	peer.on("error", console.error);
}

sendRTC();
