(function () {
    function initChart() {
        const chartElement = document.querySelector("#chartTwoCustom");
        if (!chartElement) return;

        // 1. Đọc giá trị phần trăm từ thuộc tính data-percent của thẻ HTML
        const rawPercent = chartElement.getAttribute("data-percent");
        const dynamicPercent = rawPercent ? parseFloat(rawPercent) : 0;

        const chartTwoOptions = {
            series: [dynamicPercent], // Đưa số động vào đây
            colors: ["#465FFF"],
            chart: {
                fontFamily: "Outfit, sans-serif",
                type: "radialBar",
                height: 320,
                sparkline: {
                    enabled: true
                }
            },
            plotOptions: {
                radialBar: {
                    startAngle: -90,
                    endAngle: 90,
                    hollow: {
                        size: "80%"
                    },
                    track: {
                        background: "#E4E7EC",
                        strokeWidth: "100%",
                        margin: 5
                    },
                    dataLabels: {
                        name: {
                            show: false
                        },
                        value: {
                            fontSize: "36px",
                            fontWeight: "600",
                            offsetY: 40, // Đã chỉnh lại cho chữ nằm căn đều lòng bán nguyệt
                            color: "#1D2939",
                            formatter: function (val) {
                                return val + "%";
                            }
                        }
                    }
                }
            },
            fill: {
                type: "solid",
                colors: ["#465FFF"]
            },
            stroke: {
                lineCap: "round"
            },
            labels: ["Progress"]
        };

        // 2. Kiểm tra và vẽ biểu đồ bằng thư viện CDN
        if (typeof ApexCharts !== "undefined") {
            const chart = new ApexCharts(chartElement, chartTwoOptions);
            chart.render();
        } else {
            console.error("Lỗi: Không tìm thấy thư viện ApexCharts global!");
        }
    }

    // Chạy script ngay khi DOM sẵn sàng
    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", initChart);
    } else {
        initChart();
    }
})();