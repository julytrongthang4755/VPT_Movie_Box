(function () {
  function initChartOne() {
    const chartElement = document.querySelector("#chartOneCustom");
    if (!chartElement) return;

    // 1. Đọc mảng dữ liệu số vé từ HTML (Nếu chưa có thì lấy mảng mặc định 7 ngày)
    const rawData = chartElement.getAttribute("data-sales");
    const dynamicData = rawData ? JSON.parse(rawData) : [12, 25, 18, 30, 22, 45, 35]; 

    const chartOneOptions = {
      // Đổi tên từ "Sales" thành "Số vé" để khi hover tự động hiển thị "Số vé: X"
      series: [
        {
          name: "Vé",
          data: dynamicData,
        },
      ],
      colors: ["#465fff"],
      chart: {
        fontFamily: "Outfit, sans-serif",
        type: "bar",
        height: 180,
        toolbar: {
          show: false,
        },
      },
      plotOptions: {
        bar: {
          horizontal: false,
          columnWidth: "39%",
          borderRadius: 5,
          borderRadiusApplication: "end",
        },
      },
      dataLabels: {
        enabled: false,
      },
      stroke: {
        show: true,
        width: 4,
        colors: ["transparent"],
      },
      xaxis: {
        // Đổi từ 12 tháng sang 7 ngày trong tuần (T2 -> CN)
        categories: ["Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "Chủ nhật"],
        axisBorder: {
          show: false,
        },
        axisTicks: {
          show: false,
        },
      },
      legend: {
        show: false, // Ẩn label chú thích thừa ở trên vì tiêu đề đã rõ ràng
      },
      yaxis: {
        title: false,
        // FIX LỖI "00": Ép trục Y hiển thị số nguyên, không hiển thị số thập phân lỗi
        labels: {
          formatter: function (val) {
            return Math.floor(val);
          }
        },
        min: 0
      },
      grid: {
        yaxis: {
          lines: {
            show: true,
          },
        },
      },
      fill: {
        opacity: 1,
      },
      tooltip: {
        x: {
          show: true, // Hiện Thứ mấy khi di chuột vào cột
        },
        y: {
          formatter: function (val) {
            return val + " vé"; // Hiển thị hậu tố "vé" trực quan (Ví dụ: Số vé: 20 vé)
          },
        },
      },
    };

    if (typeof ApexCharts !== "undefined") {
      const chart = new ApexCharts(chartElement, chartOneOptions);
      chart.render();
    }
  }

  // Khởi chạy khi DOM sẵn sàng
  if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", initChartOne);
  } else {
    initChartOne();
  }
})();