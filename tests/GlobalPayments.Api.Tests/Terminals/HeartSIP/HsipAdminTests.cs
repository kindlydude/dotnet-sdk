﻿using System;
using System.Diagnostics;
using GlobalPayments.Api.Entities;
using GlobalPayments.Api.Services;
using GlobalPayments.Api.Terminals;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GlobalPayments.Api.Tests.Terminals.HeartSIP {
    [TestClass]
    public class HsipAdminTests {
        IDeviceInterface _device;

        public HsipAdminTests() {
            _device = DeviceService.Create(new ConnectionConfig {
                DeviceType = DeviceType.HSIP_ISC250,
                ConnectionMode = ConnectionModes.TCP_IP,
                IpAddress = "10.12.220.130",
                Port = "12345"
            });
            Assert.IsNotNull(_device);
        }

        [TestMethod]
        public void Cancel() {
            _device.Cancel();
        }

        [TestMethod]
        public void Initialize() {
            _device.OnMessageSent += (message) => {
                Assert.AreEqual(message, "<SIP><Version>1.0</Version><ECRId>1004</ECRId><Request>GetAppInfoReport</Request></SIP>");
            };

            var response = _device.Initialize();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.DeviceResponseCode);
            Assert.IsNotNull(response.SerialNumber);
        }

        [TestMethod]
        public void OpenLane() {
            _device.OnMessageSent += (message) => {
                Assert.AreEqual(message, "<SIP><Version>1.0</Version><ECRId>1004</ECRId><Request>LaneOpen</Request></SIP>");
            };

            var response = _device.OpenLane();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.DeviceResponseCode);
        }

        [TestMethod]
        public void CloseLane() {
            _device.OnMessageSent += (message) => {
                Assert.AreEqual(message, "<SIP><Version>1.0</Version><ECRId>1004</ECRId><Request>LaneClose</Request></SIP>");
            };

            var response = _device.CloseLane();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.DeviceResponseCode);
        }

        [TestMethod]
        public void Reset() {
            _device.OnMessageSent += (message) => {
                Assert.AreEqual(message, "<SIP><Version>1.0</Version><ECRId>1004</ECRId><Request>Reset</Request></SIP>");
            };

            var response = _device.Reset();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.DeviceResponseCode);
        }

        [TestMethod, Ignore]
        public void Reboot() {
            _device.OnMessageSent += (message) => {
                Assert.AreEqual(message, "<SIP><Version>1.0</Version><ECRId>1004</ECRId><Request>Reboot</Request></SIP>");
            };

            _device.Reset();
        }

        [TestMethod]
        public void BatchClose() {
            _device.CloseLane();
            _device.OnMessageSent += (message) => {
                Assert.AreEqual(message, "<SIP><Version>1.0</Version><ECRId>1004</ECRId><Request>CloseBatch</Request></SIP>");
            };

            var response = _device.BatchClose();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.DeviceResponseCode, response.DeviceResponseText);
        }
    }
}
