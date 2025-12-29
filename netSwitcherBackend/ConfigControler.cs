using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser.Model;
using IniParser;
using System.Reflection.Metadata;

namespace netSwitcherBackend;


    public class ConfigControler
    {
    private FileIniDataParser parser;
    private IniData data;
    private String configPath;
    private String filePath;
    private String appDataPath;
    private String template;
  

    public ConfigControler() {

        this.template = @"[proxyScript]
script = blank

[isRegistered]
registered = false

[windowPosition]
x = 0
y = 0

[WindowSize]
height = 550
width = 400";




        this.appDataPath= Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        this.configPath = Path.Combine(appDataPath, "Net Switcher 2");

        //ak neexistuje vytvorí sa 

        if(!Directory.Exists(this.configPath)) Directory.CreateDirectory(this.configPath);

        this.filePath= Path.Combine(this.configPath, "config.ini");

        this.parser = new FileIniDataParser();

        if (!File.Exists(this.filePath)) File.WriteAllText(filePath, template, System.Text.Encoding.UTF8);
        this.data = parser.ReadFile(this.filePath, Encoding.UTF8);
    }


    //getters


    public String getScript()
    {
        return data["proxyScript"]["script"];
    }
    public String getHeight()
    {
        return data["WindowSize"]["height"];
    }
    public String getWidth()
    { 
        return data["WindowSize"]["width"];
    }
    public String getInitialization()
    {
        return data["isRegistered"]["registered"];
    }

    public string getPosX()
    {
        return data["windowPosition"]["x"];
    }
    public string getPosY()
    {
        return data["windowPosition"]["y"];
    }

    //setters
    public void setPosX(String x)
    {
        data["windowPosition"]["x"]=x;
        parser.WriteFile(filePath, data, Encoding.UTF8);
    }
   public void setPosY(String y)
    {
        data["windowPosition"]["y"] = y;
        parser.WriteFile(filePath, data, Encoding.UTF8);
    }
    public void setWidth(String width)
    {

        data["WindowSize"]["width"] = width;
        parser.WriteFile(filePath, data, Encoding.UTF8);

    }
    public void setHeight(String height)
    {
        data["WindowSize"]["height"] = height;
        parser.WriteFile(filePath, data, Encoding.UTF8);
    }
    public void setInitialization(String init)
    {
        data["isRegistered"]["registered"] = init;
        parser.WriteFile(filePath, data, Encoding.UTF8);

    }
    public void setScript(String script)
    {
        data["proxyScript"]["script"] = script;
        parser.WriteFile(filePath, data, Encoding.UTF8);
    }

    }

