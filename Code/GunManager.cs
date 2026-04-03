using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class GunManager : MonoBehaviour
{
    private Gun gun;
    public bool IsFiring;
    public bool isReloading = false;
    private void Awake()
    {
        gun = GetComponent<Gun>();
    }

   public void OnFireGunPressed(InputAction.CallbackContext context)
{
    if (isReloading) return;

    IsFiring = true;

    if (gun.currentMode == Gun.FireMode.Sniper)
    {
        gun.Shoot();
    }
}

    public void OnFireGunReleased(InputAction.CallbackContext context)
    {
        IsFiring = false;
    }

    private void Update()
{
    if (IsFiring && gun.currentMode == Gun.FireMode.Rifle && !isReloading)
    {
        gun.Shoot();
    }
}

    public void OnSwitchGun(InputAction.CallbackContext context)
    {
        gun.SwitchMode();
    }

   public void OnReloadGun(InputAction.CallbackContext context)
{
    if (!isReloading)
        StartCoroutine(ReloadCoroutine());
}
    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        GetComponent<Player>()?.PlayReloadAnimation();

        yield return new WaitForSeconds(1f);

        gun.Reload();
        isReloading = false;
    }
    public void OnSpecialAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Special button pressed"); // <== Thêm dòng này để kiểm tra


        Debug.Log("Shooting Special Bullet"); // <== Thêm dòng này nữa
        gun.ShootSpecial();
    }

}
