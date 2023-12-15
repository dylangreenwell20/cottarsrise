using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fireball Ability", menuName = "Abilities/Fireball")]
public class FireballAbility : Ability
{
    public override void Activate(GameObject parent)
    {
        base.Activate(parent); //run base Activate() function

        Camera cam = parent.GetComponent<PlayerMovement>().cam; //reference to camera
        Transform abilityFirePoint = parent.GetComponent<AbilityHolder>().abilityFirePoint; //reference to ability fire point
        GameObject fireballPrefab = parent.GetComponent<AbilityHolder>().fireballPrefab; //reference to fireball prefab

        PlayerStats playerStats = parent.GetComponent<PlayerStats>(); //get player stats

        int finalDamage = playerStats.DamageToDeal(abilityDamage); //calculate damage to deal

        AbilityVariables.abilityDamage = finalDamage; //store ability damage in static variable

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //create raycast from camera to where the player is looking
        RaycastHit hit; //raycast hit variable
        Vector3 destination; //vector3 variable for destination point

        if(Physics.Raycast(ray, out hit)) //if raycast hit
        {
            destination = hit.point; //set destination to hit point
        }
        else //else if raycast did not hit
        {
            destination = ray.GetPoint(30); //set destination to 30 units away
        }

        Vector3 angleOfDirection = destination - abilityFirePoint.position; //create angle of where the fireball will go

        GameObject fireball = Instantiate(fireballPrefab, abilityFirePoint.position, Quaternion.identity); //create fireball prefab
        fireball.transform.forward = angleOfDirection.normalized; //give it the angle to go forwards

        fireball.GetComponent<Rigidbody>().AddForce(angleOfDirection.normalized * abilityVelocity, ForceMode.Impulse); //add force to the fireball
    }
}
