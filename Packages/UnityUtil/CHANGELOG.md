## [1.3.0]

### New Features

- Jobx (parallel computation primitives)
  - ReverseJob: Reverse native array in parallel.
  - Hillis Steele inclusive sum scan implementation.
  - Hillis Steele inclusive min/max scan implementation.
  - Radix Sort (LSB) implementation.
- Native Containers
  - Native Increment (atomic incrementation of an int).
  - Native Add (atomic addition of an int).
  - Native Exchange (atomic exchange of an int).
- mathx:
  - `approximately_zero` function.
  - lower case for `long_axis` function.
- directional vectors added for `float2x` and `float3x` (e.g. up, down, forwad, etc.)
- Created unit tests for `Jobx` and `NativeContainers`.

### Changes

- Added package dependencies:
  - "com.unity.jobs": "0.11.0-preview.6"
  - "com.unity.burst": "1.5.5"
- Fixed `StreamingAssetFilePath` and `StreamingAssetFolderPath` stack error when opening file explorer.
- Renamed `float2_extension` and `float3_extension` to `float2x` and `float3x`.

### Bug Fixes

- Fixed Bug: Mesh Info tool throws error when mesh filter or skinned mesh renderer has no mesh/missing mesh.
- Hillis Steele inclusive sum scan does not include first element.

## [1.2.0]

### New Features

- New tool: Mesh Info (EditorWindow)
  - Under *Tools/Voxell/Mesh Info*
  - Shows mesh stats based on the GameObject/Prefab you select.
  - Recursively search for children objects inside the GameObject/Prefab for MeshFilter and SkinnedMeshRenderer.
  - Stats include:
    - Vertex Count
    - Sub Mesh Count
    - Triangle Count
  - Ping functionality allows user to ping GameObject or Mesh location.
  - Multi select support.

### Changes

- EditorStyles -> VXEditorStyles to prevent conflict with UnityEditor namespace
- EditorUtil -> VXEditorUtil (match the above naming convention)
- Moved NativeUtil from Voxell.Mathx to Voxell namespace

## [1.1.1]

### New Features

- float2_extension
  - float2 perpendicular vector
  - angle between 2 float2 vectors
- float3_extension
  - float3 LongAxis (select axis with the largest value)
- mathx
  - approximately using Unity.Mathematics instead of Mathf.
- Stride Size for all vector and matrix sizes

### Changes

- GraphicsUtil
  - Generalize dispose buffers and destroy object array functions
- NativeUtil
  - Inlining of array and list disposition functions
  - removed vector to float converter (since we can use NativeArray/NativeList.CopyFrom)

- Removed
  - VectorUtil.cs (seems unnecessary)

## [1.1.0]

### New Features

- Buffer Array disposing utilities in GraphicsUtil.
- common stride sizes in a static class called StrideSize.
- ComputeShaderUtil for handling simple parallel task.
- Sequence array generator.

## [1.0.0]

- Initial release.