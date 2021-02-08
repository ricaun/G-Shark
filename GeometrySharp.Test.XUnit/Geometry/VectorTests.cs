﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using FluentAssertions;
using GeometrySharp.Geometry;
using Xunit;
using Xunit.Abstractions;
using Plane = GeometrySharp.Geometry.Plane;
using Vector3 = GeometrySharp.Geometry.Vector3;

namespace GeometrySharp.Test.XUnit.Geometry
{
    public class VectorTests
    {
        private readonly ITestOutputHelper _testOutput;

        public VectorTests(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        public static IEnumerable<object[]> ValidateVectors =>
            new List<object[]>
            {
                new object[] { new Vector3() { 20d, -10d, 0d }, true},
                new object[] { Vector3.Unset, false},
            };

        public static IEnumerable<object[]> VectorLengths =>
            new List<object[]>
            {
                new object[] { new Vector3() { -18d, -21d, -17d }, 32.46536616149585},
                new object[] { Vector3.Unset, 0.0},
                new object[] { new Vector3() { -0d, 0d, 0d }, 0.0}
            };

        public static TheoryData<Vector3, double, Vector3> AmplifiedVectors =>
            new TheoryData<Vector3, double, Vector3>
            {
                { new Vector3{ 5, 5, 0 }, 0, new Vector3{ 0, 0, 0 }},
                { new Vector3{ 10, 10, 0 }, 15, new Vector3{ 10.606602,10.606602,0 }},
                { new Vector3{ 20, 15, 0 }, 33, new Vector3{ 26.4,19.8,0 }},
                { new Vector3{ 35, 15, 0 }, 46, new Vector3{ 42.280671,18.120288,0 }}
            };

        [Fact]
        public void It_Returns_The_Radian_Angle_Between_Two_Vectors()
        {
            Vector3 v1 = new Vector3(){20d,0d,0d};
            Vector3 v2 = new Vector3(){-10d,15d,0d};

            double angle = Vector3.AngleBetween(v1, v2);

            angle.Should().Be(2.1587989303424644);
        }

        [Fact]
        public void It_Returns_A_Reversed_Vector()
        {
            Vector3 v1 = new Vector3() { 20d, 0d, 0d };
            Vector3 vectorExpected = new Vector3() { -20d, 0d, 0d };

            Vector3 reversedVector = Vector3.Reverse(v1);

            reversedVector.Should().BeEquivalentTo(vectorExpected);
        }

        [Theory]
        [MemberData(nameof(ValidateVectors))]
        public void It_Checks_If_Vectors_Are_Valid_Or_Not(Vector3 v, bool expected)
        {
            v.IsValid().Should().Be(expected);
        }

        [Fact]
        public void It_Returns_The_Cross_Product_Between_Two_Vectors()
        {
            Vector3 v1 = new Vector3() { -10d, 5d, 10d };
            Vector3 v2 = new Vector3() { 10d, 15d, 5d };
            Vector3 crossProductExpected = new Vector3() { -125d, 150d, -200d };

            Vector3 crossProduct = Vector3.Cross(v1, v2);

            crossProduct.Should().BeEquivalentTo(crossProductExpected);
        }

        [Fact]
        public void It_Returns_The_Dot_Product_Between_Two_Vectors()
        {
            Vector3 v1 = new Vector3() { -10d, 5d, 10d };
            Vector3 v2 = new Vector3() { 10d, 15d, 5d };

            double dotProduct = Vector3.Dot(v1, v2);

            dotProduct.Should().Be(25);
        }

        [Fact]
        public void It_Returns_The_Squared_Length_Of_A_Vector()
        {
            Vector3 v1 = new Vector3() { 10d, 15d, 5d };

            double squaredLength = v1.SquaredLength();

            squaredLength.Should().Be(350);
        }

        [Theory]
        [MemberData(nameof(VectorLengths))]
        public void It_Returns_The_Length_Of_A_Vector(Vector3 v, double expectedLength)
        {
            double length = v.Length();

            length.Should().Be(expectedLength);
        }

        [Fact]
        public void It_Returns_Normalized_Vector()
        {
            Vector3 v1 = new Vector3() { -18d, -21d, -17d };
            Vector3 normalizedExpected = new Vector3() { -0.5544369932703277, -0.6468431588153823, -0.5236349380886428 };

            Vector3 normalizedVector = v1.Normalized();

            normalizedVector.Should().Equal(normalizedExpected);
        }

        [Fact]
        public void It_Returns_A_Zero1d_Vector()
        {
            Vector3 vec1D = Vector3.Zero1d(4);

            vec1D.Should().HaveCount(4);
            vec1D.Select(val => val.Should().Be(0.0));
        }

        [Fact]
        public void It_Returns_A_Zero2d_Vector()
        {
            var vec2D = Vector3.Zero2d(3,3);

            vec2D.Should().HaveCount(3);
            vec2D.Select(val => val.Should().HaveCount(3));
            vec2D.Select(val => val.Should().Contain(0.0));
        }

        [Fact]
        public void It_Returns_A_Zero3d_Vector()
        {
            var vec3D = Vector3.Zero3d(3, 3, 4);

            vec3D.Should().HaveCount(3);
            vec3D.Select(val => val.Should().HaveCount(4));
            vec3D.Select(val => val.Select(x => x.Should().Contain(0.0)));
        }

        [Theory]
        [MemberData(nameof(AmplifiedVectors))]
        public void It_Returns_An_Amplified_Vector(Vector3 vector, double amplitude, Vector3 expected)
        {
            var amplifiedVector = vector.Amplify(amplitude);

            // https://stackoverflow.com/questions/36782975/fluent-assertions-approximately-compare-a-classes-properties
            amplifiedVector.Should().BeEquivalentTo(expected, options => options
                .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 1e-6))
                .WhenTypeIs<double>());
        }

        [Fact]
        public void It_Returns_The_Addiction_Between_Two_Vectors()
        {
            var vec1 = new Vector3(){ 20, 0, 0 };
            var vec2 = new Vector3(){ -10, 15, 5 };
            var expectedVec = new Vector3() { 10, 15, 5 };

            (vec1 + vec2).Should().BeEquivalentTo(expectedVec);
        }

        [Fact]
        public void It_Returns_The_Subtraction_Between_Two_Vectors()
        {
            var vec1 = new Vector3() { 20, 0, 0 };
            var vec2 = new Vector3() { -10, 15, 5 };
            var expectedVec = new Vector3() { 30, -15, -5 };

            (vec1 - vec2).Should().BeEquivalentTo(expectedVec);
        }

        [Fact]
        public void It_Returns_The_Multiplication_Between_Two_Vectors()
        {
            var vec = new Vector3() { -10, 15, 5 };
            var expectedVec = new Vector3() { -70, 105, 35 };

            (vec * 7).Should().BeEquivalentTo(expectedVec);
        }

        [Fact]
        public void It_Returns_The_Division_Between_Two_Vectors()
        {
            var vec = new Vector3() { -10, 15, 5 };
            var expectedVec = new Vector3() { -1.428571, 2.142857, 0.714286 };

            var divisionResult = vec / 7;

            divisionResult.Select((val, i) => System.Math.Round(val, 6).Should().Be(expectedVec[i]));
        }

        [Fact]
        public void It_Returns_True_If_Vectors_Are_Equal()
        {
            var vec1 = new Vector3() { 5.982099, 5.950299, 0 };
            var vec2 = new Vector3() { 5.982099, 5.950299, 0 };

            (vec1 == vec2).Should().BeTrue();
        }

        [Fact]
        public void DistanceTo_Throws_An_Exception_If_The_Two_Vector_Have_Different_Length()
        {
            var vec1 = new Vector3() { -10, 15, 5 };
            var vec2 = new Vector3() { 10, 15 };

            Func<object> funcResult = () => vec1.DistanceTo(vec2);

            funcResult.Should().Throw<Exception>().WithMessage("The two list doesn't match in length.");
        }

        [Fact]
        public void It_Returns_The_DistanceTo_TwoVectors()
        {
            var vec1 = new Vector3() { -20, 15, 5 };
            var vec2 = new Vector3() { 10, 0, 15 };

            var distance = vec1.DistanceTo(vec2);

            distance.Should().Be(35);
        }

        [Fact]
        public void It_Checks_If_Vectors_Are_Perpendicular()
        {
            var vec = new Vector3{ -7, 10, -5 };
            var other1 = new Vector3{ 10, 7, 0 };
            var other2 = Vector3.YAxis;

            vec.IsPerpendicularTo(other1).Should().BeTrue();
            vec.IsPerpendicularTo(other2).Should().BeFalse();
        }

        [Fact]
        public void It_Returns_The_Distance_Between_A_Point_And_A_Line()
        {
            var line = new Line(new Vector3() { 0, 0, 0 }, new Vector3() { 30, 45, 0 });
            Vector3 pt = new Vector3() { 10, 20, 0 };
            double distanceExpected = 2.7735009811261464;

            double distance = pt.DistanceTo(line);

            _testOutput.WriteLine(distance.ToString());
            distance.Should().Be(distanceExpected);
        }

        [Fact]
        public void It_Checks_If_A_Point_Lies_On_A_Plane()
        {
            Plane plane = new Plane(new Vector3 { 30, 45, 0 }, new Vector3 { 30, 45, 0 });
            Vector3 pt = new Vector3() { 26.565905, 47.289396, 0.0 };

            pt.IsPointOnPlane(plane, 0.001).Should().BeTrue();
        }

        [Fact]
        public void It_Returns_The_Perpendicular_Vector()
        {
            var vector = new Vector3 {-7, 10, -5};
            var tempVec = new Vector3 {0, 1, 0};
            var vectorExpected = new Vector3 {10, 7, 0};

            var perVector = tempVec.PerpendicularTo(vector);

            perVector.Equals(vectorExpected).Should().BeTrue();
        }
    }
}
