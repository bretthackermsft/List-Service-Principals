$(function () {
    var filtertimer=0
    $("#txtFilter")
        .on("keyup", function () {
            clearTimeout(filtertimer);
            filtertimer = setTimeout(function () {
                filter();
            }, 300);
        })
    var arr = window.spArr;

    function refresh(reset) {
        $("#splist li").remove();
        $("#spdetail").html("");
        if (reset) arr = window.spArr;

        loadlist();
    }
    function filter() {
        var s = $("#txtFilter").val().toLowerCase();
        arr = $.grep(spArr, function (n, i) {
            return (n.id.toLowerCase().indexOf(s) > -1 || n.appDisplayName.toLowerCase().indexOf(s) > -1);
        });;
        refresh(false);
    }

    function loadDetail(data) {
        $("#spdetail").html("");
        for (key in data) {
            addDetail(key, data);
        }
        $.getJSON("/api/Details/GetPerms/" + data.id).done(function (res) {
            addDetail2("OAuth Permissions", res);
        });
    }
    function addDetail(key, data) {
        var d = $("<div>").addClass("detailItem row");
        $("<div>").addClass("col-sm-4").html(key).appendTo(d);
        var val = $("<div>").addClass("col-sm-8 subitem");
        var x = renderItem(data[key]);
        val.html(x).appendTo(d);
        $("#spdetail").append(d);
    }
    function addDetail2(label, data) {
        var d = $("<div>").addClass("detailItem row");
        $("<div>").addClass("col-sm-4").html(label).appendTo(d);
        var val = $("<div>").addClass("col-sm-8 subitem");
        var x = renderItem(data);
        val.html(x).appendTo(d);
        $("#spdetail").append(d);
    }

    function renderItem(item) {
        switch (typeof item) {
            case "string":
                return checkLink(item);
            case "object":
                if (item == null) {
                    return "N/A";
                }
                var res = "";
                var x;
                if (Array.isArray(item)) {
                    if (item.length == 0) return "N/A";

                    for (x in item) {
                        res += renderItem(item[x]) + "<br>";
                    }
                } else {
                    for (x in item) {
                        var s = item[x];
                        res += "<b>" + x + "</b>: " + s + "<br>";
                    }
                }
                return res;
            default:
                return checkLink(item.toString());
        }
    }
    function checkLink(item) {
        if (item.substring(0, 4) == "http") {
            var d = $("<div>").html(item + "&nbsp;&nbsp;");
            $("<a>").attr({ "target": "_blank", "href": item }).addClass("glyphicon glyphicon-globe").appendTo(d);
            return d.html();
        } else {
            return item;
        }
    }
    function loadlist() {
        $.each(arr, function (i, o) {
            var d = $("<li>")
                .addClass("spitem list-group-item")
                .attr({ "id": o.Id })
                .data("data", o)
                .on("click", function () {
                    $(".spitem").removeClass("active");
                    $(this).addClass("active");
                    loadDetail(o);
                });
            if (o.appRoles.length > 0 || o.publishedPermissionScopes.length > 0) {
                var b = $("<span>").addClass("badge");
                var s = (o.appRoles.length > 0) ? "Roles: " + o.appRoles.length : "";
                s += (s.length > 0 && o.publishedPermissionScopes.length > 0) ? "<br>" : "";
                s += (o.publishedPermissionScopes.length > 0) ? "Scopes: " + o.publishedPermissionScopes.length : "";
                b.html(s).appendTo(d);
            }
            $("<div>").addClass("spname").html(o.appDisplayName).appendTo(d);
            $("<div>").addClass("spid").html(o.id).appendTo(d);
            $("#splist").append(d);
        });
    }

    refresh(true);
});