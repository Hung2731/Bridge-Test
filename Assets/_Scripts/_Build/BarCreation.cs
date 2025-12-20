using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class BarCreation : MonoBehaviour
{
    public GameObject woodBar;
    public GameObject steelBar;

    public bool BarCreationStarted = false;
    public Bar CurrentBar;
    public GameObject BarToInstantiate;
    public Transform barParent;
    public Point CurrentStartPoint;
    public Point CurrentEndPoint;
    public GameObject PointToInstantiate;
    public Transform PointParent;
    public GameObject RoadSurfacePrefab;

    public float FixedZ = 0f;

    private Bar selectedBar = null; // Thanh hiện đang được chọn

    public MaterialDatabase materialDB;
    public LevelBudgetManager budget;
    public BarMaterialType currentMaterialType = BarMaterialType.Road;

    void StartBarCreation(Vector3 startPosition)
    {
        startPosition.z = FixedZ;
        Debug.Log("Starting bar creation");
        if (CurrentStartPoint != null)
        {
            if (false
            //GameManager_Test.AllBars.Exists(bar => bar.startPoint.PointId == CurrentStartPoint.PointId && bar.endPoint.PointId == CurrentEndPoint.PointId)
            //||
            //GameManager_Test.AllBars.Exists(bar => bar.startPoint.PointId == CurrentEndPoint.PointId && bar.endPoint.PointId == CurrentStartPoint.PointId)
            //
            )
            {
                //Debug.Log("Bar already exists with the same startPoint and endPoint.");
            }
            else
            {
                CurrentBar = Instantiate(BarToInstantiate, barParent).GetComponent<Bar>();
                CurrentBar.StartPosition = startPosition;
                CurrentBar.startPoint = CurrentStartPoint;
                CurrentBar.originalPrefab = BarToInstantiate;

                MaterialDefinition def = materialDB.Get(currentMaterialType);
                CurrentBar.SetMaterial(def);
            }
        }
        CurrentEndPoint = Instantiate(PointToInstantiate, startPosition + Vector3.up, Quaternion.identity, PointParent).GetComponent<Point>();
        Debug.Log("CurrentEndPoint " + CurrentEndPoint.PointId);
    }

    void FinishBarCreation()
    {
        Vector3Int endGrid = Vector3Int.RoundToInt(CurrentEndPoint.transform.position);

        if (GameManager_Test.AllPoints.ContainsKey(endGrid))
        {
            Debug.Log("Point exists at " + endGrid);
            Destroy(CurrentEndPoint.gameObject);
            CurrentEndPoint = GameManager_Test.AllPoints[endGrid];
        }
        else
        {
            Debug.Log("Creating new point at " + endGrid);
            GameManager_Test.AllPoints.Add(endGrid, CurrentEndPoint);

        }
        CurrentBar.endPoint = CurrentEndPoint;
        // 1. Gán lại vật liệu theo loại đang chọn
        MaterialDefinition def = materialDB.Get(currentMaterialType);
        CurrentBar.SetMaterial(def);

        // 2. Tính tiền của thanh bar
        CurrentBar.CalculateCost();

        // 3. Kiểm tra đủ tiền
        if (!budget.CanAfford(CurrentBar.cost))
        {
            Debug.Log("❌ Not enough money. Cancel bar.");
            //return;
            //Destroy(CurrentBar.gameObject);
            //Destroy(CurrentEndPoint.gameObject);
            //BarCreationStarted = false;
            //return;
        }

        // 4. Trừ tiền
        budget.Spend(CurrentBar.cost);
        Debug.Log($"Spent {CurrentBar.cost}, total spent = {budget.spent}");

        GameManager_Test.AllBars.Add(CurrentBar);

        CurrentStartPoint.ConnectedBars.Add(CurrentBar);
        CurrentEndPoint.ConnectedBars.Add(CurrentBar);

        CurrentStartPoint.ConnectedPoints.Add(CurrentEndPoint);
        CurrentEndPoint.ConnectedPoints.Add(CurrentStartPoint);

        CurrentBar.SetupJoints(CurrentStartPoint, CurrentEndPoint);

        CurrentStartPoint = CurrentEndPoint;
        StartBarCreation(CurrentStartPoint.transform.position);
    }

    void DeleteCurrentBar()
    {
        if (CurrentBar != null && CurrentBar.materialDefinition != null)
        {
            budget.Refund(CurrentBar.cost);
            Debug.Log($"Refund {CurrentBar.cost}");
        }

        AudioManager.instance.PlaySFX(SoundEffect.ButtonClick);
        GameManager_Test.AllBars.Remove(CurrentBar);
        Destroy(CurrentBar.gameObject);

        if (CurrentStartPoint.ConnectedBars.Count == 0 && CurrentStartPoint.Runtime)
        {
            GameManager_Test.AllPoints.Remove(CurrentStartPoint.PointId);
            Destroy(CurrentStartPoint.gameObject);
        }
        if (CurrentEndPoint.ConnectedBars.Count == 0 && CurrentEndPoint.Runtime)
        {
            GameManager_Test.AllPoints.Remove(CurrentEndPoint.PointId);
            Destroy(CurrentEndPoint.gameObject);
        }
    }

    private void Update()
    {
        // Bắt đầu vẽ khi nhấn chuột trái
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Point clickedPoint = hit.collider.GetComponent<Point>();

                if (!BarCreationStarted)
                {
                    BarCreationStarted = true;

                    Vector3 startPos;

                    if (clickedPoint != null)
                    {
                        // Nếu click trúng 1 point đã tồn tại
                        Debug.Log("Start from existing point: " + clickedPoint.name);
                        startPos = clickedPoint.transform.position;
                        CurrentStartPoint = clickedPoint;

                        Vector3Int id = Vector3Int.RoundToInt(startPos);
                        if (!GameManager_Test.AllPoints.ContainsKey(id))
                            GameManager_Test.AllPoints.Add(id, clickedPoint);
                    }
                    else
                    {
                        // Không trúng point nào → tạo mới
                        Vector3 mousePos = GetMouseWorldPosition();
                        startPos = RoundToGrid(mousePos);
                        Vector3Int id = Vector3Int.RoundToInt(startPos);

                        if (GameManager_Test.AllPoints.ContainsKey(id))
                        {
                            CurrentStartPoint = GameManager_Test.AllPoints[id];
                            Debug.Log("Start from existing grid point: " + CurrentStartPoint.PointId);
                        }
                        else
                        {
                            GameObject pointObj = Instantiate(PointToInstantiate, startPos, Quaternion.identity, PointParent);
                            CurrentStartPoint = pointObj.GetComponent<Point>();
                            GameManager_Test.AllPoints.Add(id, CurrentStartPoint);
                            Debug.Log("Created new start point at " + startPos);
                        }
                    }

                    // Bắt đầu tạo thanh
                    StartBarCreation(CurrentStartPoint.transform.position);
                }
                else
                {
                    FinishBarCreation();
                }
            }
        }

        // Hủy khi nhấn chuột phải
        if (Input.GetMouseButtonDown(1) && BarCreationStarted)
        {
            BarCreationStarted = false;
            DeleteCurrentBar();
        }

        if (Input.GetMouseButtonDown(1) && !BarCreationStarted)
        {
            DeleteSelectingBar();
        }

        // Cập nhật vị trí thanh đang vẽ
        if (BarCreationStarted && CurrentBar != null && CurrentEndPoint != null)
        {
            Vector3 mousePos = GetMouseWorldPosition();
            mousePos.z = FixedZ;
            Vector3 direction = mousePos - CurrentBar.StartPosition;

            Vector3 clampedPos = CurrentBar.StartPosition + Vector3.ClampMagnitude(direction, CurrentBar.maxLength);
            clampedPos = RoundToGrid(clampedPos);

            CurrentEndPoint.transform.position = clampedPos;
            CurrentEndPoint.PointId = Vector3Int.RoundToInt(clampedPos);
            CurrentBar.UpdateCreatingBar(clampedPos);
        }
    }

    Vector3 RoundToGrid(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x),
            Mathf.Round(position.y),
            FixedZ
        );
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane xyPlane = new Plane(Vector3.forward, Vector3.zero); // mặt phẳng Z=0
        float distance;
        if (xyPlane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }


    void DeleteSelectingBar()
    {
        Debug.Log("deleting selecting bar");
        // Kiểm tra xem người dùng có nhấn vào một thanh không
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Bar clickedBar = hit.collider.GetComponent<Bar>();
            if (clickedBar != null)
            {
                Debug.Log("getting bar component");
                Debug.Log("clicked bar is not null");
                // Nếu thanh được nhấn là thanh đã được chọn, xóa nó
                if (selectedBar == clickedBar)
                {
                    if (clickedBar != null)
                    {
                        budget.Refund(clickedBar.cost);

                        Debug.Log("destroy clicked bar");
                        Destroy(clickedBar.gameObject);

                    }
                    selectedBar = null; // Hủy trạng thái chọn sau khi xóa
                }
                else
                {
                    // Chọn thanh mới
                    selectedBar = clickedBar;
                    Debug.Log("Selected bar: " + clickedBar.name);
                }
                return;
            }
        }
        // Nếu không nhấn vào thanh nào, hủy trạng thái chọn
        selectedBar = null;
    }

    public void CreateBridgeSideAndRoad()
    {
        Debug.Log("=== Creating bridge sides & road surfaces (no Road filter) ===");

        // Clone toàn bộ Point
        foreach (var point in GameManager_Test.AllPoints.Values)
        {
            Vector3 clonePos = point.transform.position + Vector3.forward * -4f;
            GameObject cloneObj = Instantiate(PointToInstantiate, clonePos, Quaternion.identity, PointParent);
            Point clonePoint = cloneObj.GetComponent<Point>();
            point.clonePoint = clonePoint;

            Rigidbody rb = cloneObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (point is StaticPoint)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }
                else
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                }
            }
        }

        // Clone kết nối giữa các Point
        foreach (var point in GameManager_Test.AllPoints.Values)
        {
            Point clonePoint = point.clonePoint;
            if (clonePoint == null) continue;

            foreach (var connectedPoint in point.ConnectedPoints)
            {
                var connectedClone = connectedPoint.clonePoint;
                if (connectedClone != null && !clonePoint.ConnectedPoints.Contains(connectedClone))
                    clonePoint.ConnectedPoints.Add(connectedClone);
            }
        }

        // Clone các Bar + tạo mặt cầu giữa 2 thanh song song
        foreach (var bar in GameManager_Test.AllBars)
        {
            if (bar.startPoint == null || bar.endPoint == null) continue;

            Point startClone = bar.startPoint.clonePoint;
            Point endClone = bar.endPoint.clonePoint;
            if (startClone == null || endClone == null) continue;


            Vector3 startPos = startClone.transform.position;
            Vector3 endPos = endClone.transform.position;
            Vector3 dir = (endPos - startPos).normalized;
            Vector3 midPos = (startPos + endPos) * 0.5f;
            float length = Vector3.Distance(startPos, endPos);

            // --- Tạo thanh bar clone ---
            GameObject newBarObj = Instantiate(bar.originalPrefab, midPos, Quaternion.identity, barParent);
            newBarObj.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
            newBarObj.transform.localScale = new Vector3(length, newBarObj.transform.localScale.y, newBarObj.transform.localScale.z);

            // --- Gán point và setup joint ---
            Bar barComp = newBarObj.GetComponent<Bar>();
            barComp.startPoint = startClone;
            barComp.endPoint = endClone;
            barComp.SetupJoints(startClone, endClone);


            if (bar.barMaterialType != BarMaterialType.Road)
                continue;

            // --- Tạo mặt cầu nối giữa bar gốc & bar clone ---
            Vector3 startOriginal = bar.startPoint.transform.position;
            Vector3 endOriginal = bar.endPoint.transform.position;
            Vector3 startCloned = startClone.transform.position;
            Vector3 endCloned = endClone.transform.position;

            Vector3 forward = (endOriginal - startOriginal).normalized;
            Vector3 right = (startCloned - startOriginal).normalized;

            Vector3 center = (startOriginal + endOriginal + startCloned + endCloned) * 0.25f;
            float width = Vector3.Distance(startOriginal, startCloned);

            GameObject roadSurface = Instantiate(RoadSurfacePrefab, center, Quaternion.FromToRotation(Vector3.right, forward), barParent);
            roadSurface.transform.localScale = new Vector3(length, 0.01f, width);
            roadSurface.transform.position += Vector3.up * 0.02f;

            // --- Rigidbody cho mặt cầu ---
            Rigidbody roadRb = roadSurface.GetComponent<Rigidbody>();
            if (roadRb == null)
                roadRb = roadSurface.AddComponent<Rigidbody>();

            roadRb.mass = 0.5f;
            roadRb.useGravity = true;
            roadRb.isKinematic = false;

            // --- Tạo liên kết vật lý giữa mặt cầu & 2 thanh ---
            Rigidbody barRbA = bar.GetComponent<Rigidbody>();
            Rigidbody barRbB = newBarObj.GetComponent<Rigidbody>();

            if (barRbA != null)
            {
                AddRoadHinge(roadSurface, bar.startPoint, barRbA);
                AddRoadHinge(roadSurface, bar.endPoint, barRbA);
            }

            if (barRbB != null)
            {
                AddRoadHinge(roadSurface, startClone, barRbB);
                AddRoadHinge(roadSurface, endClone, barRbB);
            }

            //if (bar.barMaterialType == BarMaterialType.Road) {
            //    bar.gameObject.SetActive(false);
            //}
            Debug.Log($"✅ Created surface between {bar.name} and its clone");
        }

        Debug.Log("✅ Bridge sides + surfaces created successfully!");
    }

    void AddRoadHinge(GameObject road, Point point, Rigidbody barRb)
    {
        HingeJoint hj = road.AddComponent<HingeJoint>();
        hj.connectedBody = barRb;

        hj.autoConfigureConnectedAnchor = false;

        // world position của point
        Vector3 worldPos = point.transform.position;

        // anchor trên road (local)
        hj.anchor = road.transform.InverseTransformPoint(worldPos);

        // anchor trên bar (local)
        hj.connectedAnchor = barRb.transform.InverseTransformPoint(worldPos);

        // cầu nằm trên mặt phẳng XY → xoay quanh Z
        hj.axis = Vector3.forward;

        hj.useLimits = false;
        hj.enablePreprocessing = false;
        hj.breakForce = 700f;
    }

}
