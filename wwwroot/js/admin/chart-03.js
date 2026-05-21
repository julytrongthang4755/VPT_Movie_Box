(function () {
    function initChartThree() {
        const chartElement = document.querySelector("#chartThree");
        if (!chartElement) return;

        const chartThreeOptions = {
            series: [
                {
                    name: "Sales",
                    data: [180, 190, 170, 160, 175, 165, 170, 205, 230, 210, 240, 235],
                },
                {
                    name: "Revenue",
                    data: [40, 30, 50, 40, 55, 40, 70, 100, 110, 120, 150, 140],
                },
            ],
            legend: {
                show: false,
            },
            colors: ["#465FFF", "#9CB9FF"],
            chart: {
                fontFamily: "Outfit, sans-serif",
                height: 310,
                type: "area",
                toolbar: {
                    show: false,
                },
            },
            fill: {
                gradient: {
                    enabled: true,
                    opacityFrom: 0.55,
                    opacityTo: 0,
                },
            },
            stroke: {
                curve: "straight",
                width: ["2", "2"],
            },
            markers: {
                size: 0,
            },
            grid: {
                xaxis: { lines: { show: false } },
                yaxis: { lines: { show: true } },
            },
            dataLabels: {
                enabled: false,
            },
            tooltip: {
                x: { format: "dd MMM yyyy" },
            },
            xaxis: {
                type: "category",
                categories: [
                    "Jan", "Feb", "Mar", "Apr", "May", "Jun",
                    "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
                ],
                axisBorder: { show: false },
                axisTicks: { show: false },
                tooltip: false,
            },
            yaxis: {
                title: { style: { fontSize: "0px" } },
            },
        };

        if (typeof ApexCharts !== "undefined") {
            const chartThree = new ApexCharts(chartElement, chartThreeOptions);
            chartThree.render();

            // --- XỬ LÝ 3 NÚT BẤM ĐỔI MÀU VÀ ẨN/HIỆN ĐƯỜNG ---
            const btnOverview = document.getElementById("btn-overview");
            const btnSales = document.getElementById("btn-sales");
            const btnRevenue = document.getElementById("btn-revenue");
            const allBtns = [btnOverview, btnSales, btnRevenue];

            // Hàm con để xử lý đổi class màu sắc khi click nút
            function setActiveButton(activeBtn) {
                allBtns.forEach(btn => {
                    if (btn) {
                        btn.className = "text-gray-500 dark:text-gray-400 text-theme-sm rounded-md px-3 py-2 font-medium hover:text-gray-900 dark:hover:text-white";
                    }
                });
                if (activeBtn) {
                    activeBtn.className = "shadow-theme-xs text-gray-900 dark:text-white bg-white dark:bg-gray-800 text-theme-sm rounded-md px-3 py-2 font-medium hover:text-gray-900 dark:hover:text-white";
                }
            }

            if (btnOverview) {
                btnOverview.addEventListener("click", function () {
                    setActiveButton(btnOverview);
                    chartThree.showSeries("Sales");
                    chartThree.showSeries("Revenue");
                });
            }

            if (btnSales) {
                btnSales.addEventListener("click", function () {
                    setActiveButton(btnSales);
                    chartThree.showSeries("Sales");
                    chartThree.hideSeries("Revenue");
                });
            }

            if (btnRevenue) {
                btnRevenue.addEventListener("click", function () {
                    setActiveButton(btnRevenue);
                    chartThree.hideSeries("Sales");
                    chartThree.showSeries("Revenue");
                });
            }
        }
    }

    // Tự động kích hoạt khi trang web tải xong (Y hệt file chart-01.js)
    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", initChartThree);
    } else {
        initChartThree();
    }
})();