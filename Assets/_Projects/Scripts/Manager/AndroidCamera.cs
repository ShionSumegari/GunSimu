using UnityEngine;

public class AndroidCamera
{

	AndroidJavaObject camera = null;

	public AndroidCamera()
	{
		WebCamDevice[] devices = WebCamTexture.devices;
		Debug.Log("Camera Name:" + devices[0].name);

		open();
	}

	public void open()
	{
		if (camera == null)
		{
#if (UNITY_ANDROID && !UNITY_EDITOR)
						AndroidJavaClass cameraClass = new AndroidJavaClass("android.hardware.Camera");
						camera = cameraClass.CallStatic<AndroidJavaObject>("open");
#endif
        }
    }

	public void release()
	{
		if (camera != null)
		{
			LEDOff();

			camera.Call("release");
			camera = null;
		}
	}

	public void startPreview()
	{
		if (camera != null)
		{
			Debug.Log("AndroidCamera::startPreview()");
			camera.Call("startPreview");
		}
	}

	public void stopPreview()
	{
		if (camera != null)
		{
			Debug.Log("AndroidCamera::stopPreview()");
			LEDOff();
			camera.Call("stopPreview");
		}
	}

	// Flashモードの設定	
	private void setFlashMode(string mode)
	{
		if (camera != null)
		{
			AndroidJavaObject cameraParameters = camera.Call<AndroidJavaObject>("getParameters");
			cameraParameters.Call("setFlashMode", mode);
			camera.Call("setParameters", cameraParameters);
			Debug.Log("FlashON");
        }
        else
        {
			Debug.Log("FlashOFF");
		}
	}

	// LED点灯
	public void LEDOn()
	{
		setFlashMode("torch");
	}

	// LED消灯
	public void LEDOff()
	{
		//setFlashMode("off");
	}
}