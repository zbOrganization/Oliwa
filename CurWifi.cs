using System;

public class CurWifi
{
    private string sSID;
    private string bSSID;
    private int signal;
    public CurWifi(string SSID, string BSSID,int signal)
    {
        this.bSSID = BSSID;
        this.sSID = SSID;
        this.signal = signal;
	
	}

    public int Signal
    {


        get
        {

            return this.signal;
        }

    }
    public string BSSID
    {


        get
        {

            return this.bSSID;
        }

    }
    public string SSID
    {
        get
        {

            return this.sSID;
        }

    }
}
