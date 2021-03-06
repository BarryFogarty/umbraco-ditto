﻿namespace Our.Umbraco.Ditto.Tests
{
    using NUnit.Framework;
    using Our.Umbraco.Ditto.Tests.Mocks;

    [TestFixture]
    public class MultipleConversionHandlerTests
    {
        public class MyModel
        {
            public string Name { get; set; }
        }

        public class MyModelConversionHandler : DittoConversionHandler<MyModel>
        {
            public override void OnConverted()
            {
                Model.Name = "foo";
            }
        }

        public class MyModelConversionHandler2 : DittoConversionHandler<MyModel>
        {
            public override void OnConverted()
            {
                Model.Name += " bar";
            }
        }

        [Test]
        public void Multiple_Conversion_Handlers_Registered_Same_Type()
        {
            Ditto.RegisterConversionHandler<MyModel, MyModelConversionHandler>();
            Ditto.RegisterConversionHandler<MyModel, MyModelConversionHandler2>();

            var content = new PublishedContentMock();

            var model = content.As<MyModel>();

            Assert.That(model, Is.Not.Null);
            Assert.That(model.Name, Is.EqualTo("foo bar"));
        }
    }
}