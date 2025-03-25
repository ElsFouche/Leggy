using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdhouseManager : MonoBehaviour
{
    public List<Collider> snapZones; // List of BoxColliders (snap zones)
    public float rotationTolerance = 10f; // Rotation tolerance (degrees)
    public float snapDistance = 1f; // Distance to check for snapping
    private int piecesAssembled = 0;

    public BirdhousePiece[] pieces; // Array of all the birdhouse pieces

    void Update()
    {
        piecesAssembled = 0;
        foreach (var piece in pieces)
        {
            // Check if the piece has entered any snap zone
            foreach (var zone in snapZones)
            {
                if (zone.bounds.Contains(piece.transform.position) && IsInCorrectRotation(piece.transform.rotation, zone.transform.rotation))
                {
                    piece.SnappedToZone(zone);
                    piecesAssembled++;
                    break;
                }
            }
        }

        // Check if all pieces are assembled
        if (piecesAssembled == pieces.Length)
        {
            // Puzzle completed!
            Debug.Log("Birdhouse Assembled!");
            // Trigger the next event (e.g., open a door, play sound, etc.)
        }
    }

    bool IsInCorrectRotation(Quaternion pieceRotation, Quaternion zoneRotation)
    {
        // Calculate the difference in rotation (Euler angles) and check within tolerance
        Vector3 eulerDiff = pieceRotation.eulerAngles - zoneRotation.eulerAngles;

        // Normalize the angles to be within -180 to 180 range
        eulerDiff = new Vector3(
            Mathf.DeltaAngle(0, eulerDiff.x),
            Mathf.DeltaAngle(0, eulerDiff.y),
            Mathf.DeltaAngle(0, eulerDiff.z)
        );

        // Check if the rotation difference is within the acceptable tolerance
        return Mathf.Abs(eulerDiff.x) <= rotationTolerance &&
               Mathf.Abs(eulerDiff.y) <= rotationTolerance &&
               Mathf.Abs(eulerDiff.z) <= rotationTolerance;
    }
}

public class BirdhousePiece : MonoBehaviour
{
    public bool isSnapped = false;

    // Function to snap the piece to the target snap zone
    public void SnappedToZone(Collider snapZone)
    {
        // Set the piece as snapped
        isSnapped = true;

        // Parent the piece to the snap zone so it becomes part of the structure
        transform.SetParent(snapZone.transform);

        // Lock the position and rotation if needed
        transform.position = snapZone.transform.position;
        transform.rotation = snapZone.transform.rotation;

        // Disable physics if you want the piece to stay static once snapped
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Makes the piece not affected by physics anymore
        }
    }
}
