using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]

public class LightSource : MonoBehaviour

{
 
    private GameObject Celestial;
    private GameObject Planet;
	private GameObject Atmosphere;
	private GameObject Rings;
    private Transform IlluminationSource;
	private MaterialPropertyBlock _propBlockPlanet;
	private MaterialPropertyBlock _propBlockAtmosphere;
	private MaterialPropertyBlock _propBlockRings;
    private Renderer PlanetM;
	private Renderer RingsM;
    private Renderer AtmosphereM;
	private float lastAngle = 0;

	private GameObject Sun;
		
public void Start ()

{
        Celestial = this.gameObject;
		
       
		Sun = GameObject.FindGameObjectWithTag("MainLight");
		
		
        Planet = transform.Find("Planet").gameObject;
		PlanetM = Planet.GetComponent<Renderer>();
		
	
	
		Atmosphere = transform.Find("Atmosphere").gameObject;
		AtmosphereM = Atmosphere.GetComponent<Renderer>();
		
			
		Rings = transform.Find("Rings").gameObject;
		RingsM = Rings.GetComponent<Renderer>();
		

}

    public void Update()

    {
    	if (Sun != null)
		{
			
	   IlluminationSource = Sun.transform;
	   //Vector3 targetDir = IlluminationSource.position - Celestial.transform.position;
	   Vector3 targetDir = Vector3.zero;
	   float angle = IlluminationSource.localRotation.eulerAngles.x;
	   if (angle >= 270 && angle <= 360)
	   {
		   angle -= 270;
	   }
	   else // 0-90
	   {
		   angle += 90;
	   }
	   targetDir.x = Mathf.Cos(Mathf.Deg2Rad * IlluminationSource.localRotation.eulerAngles.x) * 10.0f;
	   targetDir.y = Mathf.Sin(Mathf.Deg2Rad * IlluminationSource.localRotation.eulerAngles.x) * -10.0f;
	   if (lastAngle > angle)
	   {
		   targetDir.x *= -1;
	   }
	   lastAngle = angle;
	   targetDir *= -1;

	   

 _propBlockPlanet = new MaterialPropertyBlock();
	   PlanetM.GetPropertyBlock(_propBlockPlanet);
	   _propBlockPlanet.SetVector("_LightSource", targetDir);
       PlanetM.SetPropertyBlock(_propBlockPlanet);
	   
_propBlockAtmosphere = new MaterialPropertyBlock();	   
	   AtmosphereM.GetPropertyBlock(_propBlockAtmosphere);
	   _propBlockAtmosphere.SetVector("_LightSourceAtmo", targetDir);
       AtmosphereM.SetPropertyBlock(_propBlockAtmosphere);
	   
_propBlockRings = new MaterialPropertyBlock();	   
	   RingsM.GetPropertyBlock(_propBlockRings);
	   _propBlockRings.SetVector("_LightSourceRings", targetDir);
       RingsM.SetPropertyBlock(_propBlockRings);
    }
	}
}

