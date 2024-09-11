alert("coucou1");
function SortHtmlTable(currentID, tblOfrs) {
    alert("coucouSort");
    var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;
    table = document.getElementById("offers" + currentID);
    switching = true;
    dir = "asc";

    while (switching) {
        switching = false;
        rows = table.rows;
        for (i = 1; i < (rows.length - 2); i += 2) {
            shouldSwitch = false;
            x = tblOfrs[i];
            y = tblOfrs[i + 2];
            if (dir == "asc") {
                if (x > y) {
                    shouldSwitch = true;
                    break;
                }
            } else if (dir == "desc") {
                if (x < y) {
                    shouldSwitch = true;
                    break;
                }
            }
        }
        if (shouldSwitch) {
            rows[i].parentNode.insertBefore(rows[i + 2], rows[i]);
            rows[i].parentNode.insertBefore(rows[i + 3], rows[i + 1]);
            switching = true;
            switchcount++;
        } else {
            if (switchcount == 0 && dir == "asc") {
                dir = "desc";
                switching = true;
            }
        }
    }
}
