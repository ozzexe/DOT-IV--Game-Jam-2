using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{
    [SerializeField]
    private LayerMask pickableLayerMask;

    [SerializeField]
    private Transform playerCameraTransform;

    [SerializeField]
    [Min(1)]
    private float hitRange = 3;

    [SerializeField]
    private Transform pickUpParent;

    [SerializeField]
    private GameObject inHandItem;

    [SerializeField]
    private float throwForce;

    [SerializeField]
    private InputActionReference interactionInput, dropInput, useInput, placeInput;

    private RaycastHit hit;

    private void Start()
    {
        interactionInput.action.Enable();
        dropInput.action.Enable();
        useInput.action.Enable();
        placeInput.action.Enable();

        interactionInput.action.performed += Interact;
        dropInput.action.performed += Drop;
        useInput.action.performed += Use;
        placeInput.action.performed += PlaceObject; // "R" tuþuna basýldýðýnda çaðýr
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        if (inHandItem != null) return;

        if (hit.collider != null)
        {
            GameObject pickedObject = hit.collider.gameObject;
            Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
            Collider col = pickedObject.GetComponent<Collider>();

            if (pickedObject.GetComponent<Food>())
            {
                inHandItem = pickedObject;
                inHandItem.transform.position = Vector3.zero;
                inHandItem.transform.rotation = Quaternion.identity;
                inHandItem.transform.SetParent(pickUpParent.transform, false);

                if (rb != null)
                    rb.isKinematic = true;

                if (col != null)
                    col.enabled = false;

                return;
            }

            if (pickedObject.GetComponent<Item>())
            {
                Debug.Log("Kullanýldý!");
                inHandItem = pickedObject;
                inHandItem.transform.SetParent(pickUpParent.transform, true);

                if (rb != null)
                    rb.isKinematic = true;

                return;
            }
        }
    }

    private void Use(InputAction.CallbackContext obj)
    {

    }

    private void Drop(InputAction.CallbackContext obj)
    {
        if (inHandItem == null) return;

        Rigidbody rb = inHandItem.GetComponent<Rigidbody>();
        Collider col = inHandItem.GetComponent<Collider>();

        inHandItem.transform.SetParent(null);

        if (col != null)
            col.enabled = true;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(playerCameraTransform.forward * throwForce, ForceMode.Impulse);
        }

        inHandItem = null;
    }

    private void Update()
    {
        Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * hitRange, Color.red);

        if (hit.collider != null)
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(false);

        if (inHandItem != null) return;

        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, hitRange, pickableLayerMask))
        {
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);
        }
    }

    private void PlaceObject(InputAction.CallbackContext obj)
    {
        if (inHandItem == null) return;

        Transform closestPoint = FindClosestPlacementPoint();
        if (closestPoint != null)
        {
            PlacementPoint pointScript = closestPoint.GetComponent<PlacementPoint>();

            if (pointScript != null && !pointScript.isOccupied) // Nokta boþ mu?
            {
                inHandItem.transform.position = closestPoint.position;
                inHandItem.transform.rotation = closestPoint.rotation;
                inHandItem.transform.SetParent(null); // Nesne artýk karaktere baðlý deðil

                pointScript.isOccupied = true; // Bu nokta artýk dolu
                inHandItem = null; // Elimiz boþ
            }
        }
    }

    private Transform FindClosestPlacementPoint()
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("PlacementPoint");
        Transform closestPoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject point in points)
        {
            PlacementPoint pointScript = point.GetComponent<PlacementPoint>();
            if (pointScript != null && !pointScript.isOccupied) // Sadece boþ noktalarý kontrol et
            {
                float distance = Vector3.Distance(inHandItem.transform.position, point.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = point.transform;
                }
            }
        }

        return closestPoint;
    }
}
