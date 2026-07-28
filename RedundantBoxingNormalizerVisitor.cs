using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Grammophone.DataAccess.EntityFramework
{
	/// <summary>
	/// Removes redundant boxing conversions that appear as the value of a member assignment,
	/// rewriting <c>member = (object)valueTypeExpression</c> into <c>member = valueTypeExpression</c>
	/// whenever the member is assignable from the operand's type.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Entity Framework 6's LINQ-to-Entities translator rejects an explicit cast of a primitive to
	/// <see cref="object"/> in a member initialization. Some third-party queryable builders (notably Kendo UI's
	/// grid aggregate builder) insert exactly such an explicit <c>Convert(x, object)</c> around value-type group
	/// keys whenever the underlying provider is not one of a hard-coded set of recognized Entity Framework
	/// provider type names — Entity Framework's own providers are special-cased to receive the implicit binding
	/// <c>member = x</c> instead, which the translator accepts.
	/// </para>
	/// <para>
	/// Because the portable query abstraction wraps the native provider behind its own provider type, such
	/// builders do not recognize it and emit the explicit box, which then fails once the query is forwarded to
	/// the native Entity Framework 6 provider. Stripping the redundant box restores the exact shape the native
	/// provider would have received, without impersonating any specific provider. The rewrite is a semantic
	/// no-op: assigning a value-type expression to a reference-typed member boxes implicitly. Entity Framework
	/// Core tolerates the explicit box, so this normalization is applied only by the Entity Framework 6 backend.
	/// </para>
	/// </remarks>
	public class RedundantBoxingNormalizerVisitor : ExpressionVisitor
	{
		#region Protected methods

		/// <inheritdoc/>
		protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
		{
			if (node == null) throw new ArgumentNullException(nameof(node));

			var assignment = base.VisitMemberAssignment(node);

			if (assignment.Expression is UnaryExpression conversion
				&& (conversion.NodeType == ExpressionType.Convert || conversion.NodeType == ExpressionType.ConvertChecked)
				&& conversion.Method == null
				&& conversion.Type == typeof(object)
				&& conversion.Operand.Type.IsValueType)
			{
				var memberType = GetMemberType(assignment.Member);

				if (memberType != null && memberType.IsAssignableFrom(conversion.Operand.Type))
				{
					return Expression.Bind(assignment.Member, conversion.Operand);
				}
			}

			return assignment;
		}

		#endregion

		#region Private methods

		private static Type GetMemberType(MemberInfo member)
		{
			switch (member)
			{
				case PropertyInfo propertyInfo:
					return propertyInfo.PropertyType;

				case FieldInfo fieldInfo:
					return fieldInfo.FieldType;

				default:
					return null;
			}
		}

		#endregion
	}
}
