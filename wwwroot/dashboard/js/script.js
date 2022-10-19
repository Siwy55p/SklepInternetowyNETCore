$('#add_more').click(function () {
    "use strict";
    $(this).before($("<div/>", {
        id: 'filediv'
    }).fadeIn('slow').append(
        $("<input/>", {
            name: 'file[]',
            type: 'file',
            id: 'file',
            multiple: 'multiple',
            accept: 'image/*'
        })
    ));
});

$('#upload').click(function (e) {
    "use strict";
    e.preventDefault();

    if (window.filesToUpload.length === 0 || typeof window.filesToUpload === "undefined") {
        alert("No files are selected.");
        return false;
    }

    // Now, upload the files below...
    // https://developer.mozilla.org/en-US/docs/Using_files_from_web_applications#Handling_the_upload_process_for_a_file.2C_asynchronously
});

deletePreview = function (ele, i) {
    "use strict";
    try {
        $(ele).parent().remove();
        window.filesToUpload.splice(i, 1);
    } catch (e) {
        console.log(e.message);
    }
}

$("#file").on('change', function () {
    "use strict";

    // create an empty array for the files to reside.
    window.filesToUpload = [];

    if (this.files.length >= 1) {
        $("[id^=previewImg]").remove();
        $.each(this.files, function (i, img) {
            var reader = new FileReader(),
                newElement = $("<div id='previewImg" + i + "' class='previewBox'><img /></div>"),
                deleteBtn = $("<span class='delete' onClick='deletePreview(this, " + i + ")'>X</span>").prependTo(newElement),
                preview = newElement.find("img");

            reader.onloadend = function () {
                preview.attr("src", reader.result);
                preview.attr("alt", img.name);
            };

            try {
                window.filesToUpload.push(document.getElementById("file").files[i]);
            } catch (e) {
                console.log(e.message);
            }

            if (img) {
                reader.readAsDataURL(img);
            } else {
                preview.src = "";
            }

            newElement.appendTo("#filediv");
        });
    }
});














const charts = document.querySelectorAll(".chart");

charts.forEach(function (chart) {
  var ctx = chart.getContext("2d");
  var myChart = new Chart(ctx, {
    type: "bar",
    data: {
      labels: ["Red", "Blue", "Yellow", "Green", "Purple", "Orange"],
      datasets: [
        {
          label: "# of Votes",
          data: [12, 19, 3, 5, 2, 3],
          backgroundColor: [
            "rgba(255, 99, 132, 0.2)",
            "rgba(54, 162, 235, 0.2)",
            "rgba(255, 206, 86, 0.2)",
            "rgba(75, 192, 192, 0.2)",
            "rgba(153, 102, 255, 0.2)",
            "rgba(255, 159, 64, 0.2)",
          ],
          borderColor: [
            "rgba(255, 99, 132, 1)",
            "rgba(54, 162, 235, 1)",
            "rgba(255, 206, 86, 1)",
            "rgba(75, 192, 192, 1)",
            "rgba(153, 102, 255, 1)",
            "rgba(255, 159, 64, 1)",
          ],
          borderWidth: 1,
        },
      ],
    },
    options: {
      scales: {
        y: {
          beginAtZero: true,
        },
      },
    },
  });
});

$(document).ready(function () {
  $(".data-table").each(function (_, table) {
    $(table).DataTable();
  });
});
