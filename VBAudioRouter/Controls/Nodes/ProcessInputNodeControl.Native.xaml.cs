using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.System.Diagnostics.ToolHelp;
using static Windows.Win32.PInvoke;

namespace VBAudioRouter.Controls.Nodes;

partial class ProcessInputNodeControl
{
    public static List<ProcessTreeNode> ProcessSnapshot()
    {
        Dictionary<uint, ProcessTreeNode> nodes = [];
        foreach (var process in new ProcessEnumerator())
        {
            if (process.th32ProcessID == 0)
                continue;

            ref var parent = ref CollectionsMarshal.GetValueRefOrAddDefault(nodes, process.th32ParentProcessID, out _);
            parent ??= new(process.th32ParentProcessID);

            ref var current = ref CollectionsMarshal.GetValueRefOrAddDefault(nodes, process.th32ProcessID, out _);
            current ??= new(process.th32ProcessID);

            parent.Children.Add(current);

            current.Name = GetDisplayName(process);
            current.Parent = process.th32ParentProcessID;
        }

        List<ProcessTreeNode> result = [];
        foreach (var node in nodes.Values)
        {
            if (node.Parent is null)
                result.AddRange(node.Children);
        }

        return result;

        static string GetDisplayName(PROCESSENTRY32W process)
            => process.szExeFile.ToString();
    }

    private ref struct ProcessEnumerator
    {
        bool _isFirst = true;
        HANDLE _snapshot;
        public ProcessEnumerator()
        {
            _snapshot = CreateToolhelp32Snapshot(CREATE_TOOLHELP_SNAPSHOT_FLAGS.TH32CS_SNAPPROCESS, 0);
            if (_snapshot.IsNull)
                throw new InvalidOperationException("Failed to create process snapshot.");
        }

        public PROCESSENTRY32W Current { get; private set; }

        public unsafe bool MoveNext()
        {
            bool hasNext;
            PROCESSENTRY32W entry = new()
            {
                dwSize = (uint)sizeof(PROCESSENTRY32W)
            };

            if (_isFirst)
            {
                _isFirst = false;
                hasNext = Process32FirstW(_snapshot, &entry);
            }
            else
            {
                hasNext = Process32NextW(_snapshot, &entry);
            }

            Current = entry;
            return hasNext;
        }

        public void Dispose()
        {
            CloseHandle(_snapshot);
            _snapshot = HANDLE.Null;
        }

        public readonly ProcessEnumerator GetEnumerator()
            => this;
    }
}

public sealed partial class ProcessTreeNode(uint id, string name) : ObservableObject
{
    public ProcessTreeNode(uint id) : this(id, "Unknown") { }

    public uint Id { get; set; } = id;
    public uint? Parent { get; set; }
    public string Name { get; set; } = name;

    public readonly ObservableCollection<ProcessTreeNode> Children = [];
}
